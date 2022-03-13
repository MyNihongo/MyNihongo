using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MyNihongo.WebApi.Models.Auth;
using MyNihongo.WebApi.Services;
using MyNihongo.WebApi.Utils.Extensions;
using static MyNihongo.WebApi.Resources.Const.ApiConst;

namespace MyNihongo.WebApi.Auth;

public sealed class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	private readonly IMutableUserSession _mutableUserSession;
	private readonly ITokenService _tokenService;

	public AuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IMutableUserSession mutableUserSession, ITokenService tokenService)
		: base(options, logger, encoder, clock)
	{
		_mutableUserSession = mutableUserSession;
		_tokenService = tokenService;
	}

	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.TryGetValue(Headers.ClientInfo, out var stringValues) || !stringValues.TryGetSingle(out var clientInfo))
			return Fail();

		if (!Request.Headers.TryGetValue("Cookie", out stringValues))
			return Fail();

		string accessToken = string.Empty, refreshToken = string.Empty;
		for (var i = 0; i < stringValues.Count; i++)
		{
			var separatorIndex = stringValues[i].IndexOf('=');
			if (separatorIndex == -1)
				continue;

			if (stringValues[i].HasSequence(Cookies.AccessToken))
				accessToken = stringValues[i][(separatorIndex + 1)..];
			else if (stringValues[i].HasSequence(Cookies.RefreshToken))
				refreshToken = stringValues[i][(separatorIndex + 1)..];
		}

		if (string.IsNullOrEmpty(refreshToken))
			return Fail();

		var ipAddress = Context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;

		if (string.IsNullOrEmpty(accessToken))
		{
			var refreshParams = new AuthConnectionValidationParams
			{
				Token = refreshToken,
				IpAddress = ipAddress,
				ClientInfo = clientInfo
			};

			var validatedByRefresh = await _tokenService.ValidateRefreshTokenAsync(refreshParams)
				.ConfigureAwait(false);

			if (validatedByRefresh == null)
			{
				// Reset it on the client in order not to perform the validation again
				_mutableUserSession.SetTokens(AuthGenerateTokensResult.EmptyRefresh);
				return Fail();
			}

			var tokenData = await _tokenService.RegenerateTokensAsync(validatedByRefresh)
				.ConfigureAwait(false);

			_mutableUserSession.SetTokens(tokenData);

			return Ok(validatedByRefresh.UserId);
		}

		var accessParams = new AuthConnectionValidationParams
		{
			Token = accessToken,
			IpAddress = ipAddress,
			ClientInfo = clientInfo
		};

		var validatedByAccess = await _tokenService.ValidateAccessTokenAsync(accessParams)
			.ConfigureAwait(false);

		if (validatedByAccess.HasValue)
			return Ok(validatedByAccess.Value);

		// Reset it on the client in order not to perform the validation again
		_mutableUserSession.SetTokens(AuthGenerateTokensResult.EmptyAccess);
		return Fail();
	}

	private static AuthenticateResult Fail() =>
		AuthenticateResult.Fail("Unauthorized");

	private AuthenticateResult Ok(long userId)
	{
		var ticket = new AuthenticationTicket(
			new ClaimsPrincipal(new GenericIdentity(userId.ToString())),
			new AuthenticationProperties(),
			AuthPolicy);

		_mutableUserSession.SetUserId(userId);
		return AuthenticateResult.Success(ticket);
	}
}