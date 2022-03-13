using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MyNihongo.WebApi.Infrastructure;
using MyNihongo.WebApi.Infrastructure.Auth;
using MyNihongo.WebApi.Models;
using MyNihongo.WebApi.Models.Auth;
using MyNihongo.WebApi.Resources.Const;
using MyNihongo.WebApi.Utils.Extensions;

namespace MyNihongo.WebApi.Services;

public sealed class TokenService : ITokenService
{
	private readonly IConfiguration _configuration;
	private readonly IDateTimeService _dateTimeService;
	private readonly IAuthService _authService;

	public TokenService(
		IConfiguration configuration,
		IDateTimeService dateTimeService,
		IAuthService authService)
	{
		_configuration = configuration;
		_dateTimeService = dateTimeService;
		_authService = authService;
	}

	public async Task<AuthGenerateTokensResult> RegenerateTokensAsync(AuthConnectionValidationResult validationResult, CancellationToken ct = default)
	{
		var accessOptions = _configuration.GetAccessTokenOptions();
		var refreshOptions = _configuration.GetRefreshTokenOptions();
		var instant = _dateTimeService.GetInstant();

		var parameters = new AuthRecreateTokensParams
		{
			ConnectionId = validationResult.ConnectionId,
			TicksAccessValidTo = instant.GetTicksWithOffset(accessOptions.Expires),
			TicksRefreshValidTo = instant.GetTicksWithOffset(refreshOptions.Expires)
		};

		var (accessTokenId, refreshTokenId) = await _authService.RegenerateTokensAsync(parameters, ct)
			.ConfigureAwait(false);

		var accessToken = GenerateToken(accessTokenId, validationResult.UserId, accessOptions);
		var refreshToken = GenerateToken(refreshTokenId, validationResult.UserId, refreshOptions);

		return new AuthGenerateTokensResult(accessToken, refreshToken);
	}

	public async ValueTask<long?> ValidateAccessTokenAsync(AuthConnectionValidationParams parameters, CancellationToken ct = default)
	{
		var options = _configuration.GetAccessTokenOptions();
		if (!TryGetJwtClaims(parameters.Token, options, out var claims))
			return null;

		var @params = new AuthSetConnectionInfoParams(claims.TokenId, claims.UserId)
		{
			IpAddress = parameters.IpAddress,
			ClientInfo = parameters.ClientInfo
		};

		var connectionId = await _authService.TrySetConnectionInfoAsync(@params, ct)
			.ConfigureAwait(false);

		return connectionId.HasValue
			? claims.UserId
			: null;
	}

	public async ValueTask<AuthConnectionValidationResult?> ValidateRefreshTokenAsync(AuthConnectionValidationParams parameters, CancellationToken ct = default)
	{
		var options = _configuration.GetRefreshTokenOptions();
		if (!TryGetJwtClaims(parameters.Token, options, out var claims))
			return null;

		var @params = new AuthSetConnectionInfoParams(claims.TokenId, claims.UserId)
		{
			IpAddress = parameters.IpAddress,
			ClientInfo = parameters.ClientInfo
		};

		var connectionId = await _authService.TrySetConnectionInfoAsync(@params, ct)
			.ConfigureAwait(false);

		return connectionId.HasValue
			? new AuthConnectionValidationResult { ConnectionId = connectionId.Value, UserId = claims.UserId }
			: null;
	}

	private AuthGenerateTokensResult.TokenData GenerateToken(in Guid tokenId, in long userId, in ConfigurationTokenOptions options)
	{
		var validIssuer = _configuration.GetValidIssuer();
		var credentials = new SigningCredentials(options.SigningKey, SecurityAlgorithms.HmacSha256Signature);

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ApiConst.Claims.TokenId, tokenId.ToString()),
				new(ApiConst.Claims.UserId, userId.ToString())
			}),
			Expires = _dateTimeService.GetUtcNow().Add(options.Expires),
			Issuer = validIssuer,
			SigningCredentials = credentials
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenValue = tokenHandler.WriteToken(token);
		return new AuthGenerateTokensResult.TokenData(tokenValue, options.Expires);
	}

	private bool TryGetJwtClaims(in string token, in ConfigurationTokenOptions options, out AuthTokenClaimsRecord claims)
	{
		var validIssuer = _configuration.GetValidIssuer();

		var tokenHandler = new JwtSecurityTokenHandler();
		var parameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			ValidateIssuer = true,
			RequireExpirationTime = true,
			ValidateAudience = false,
			ValidIssuer = validIssuer,
			IssuerSigningKey = options.SigningKey,
			ClockSkew = TimeSpan.Zero
		};

		try
		{
			var principal = tokenHandler.ValidateToken(token, parameters, out _);
			if (principal != null)
			{
				claims = new AuthTokenClaimsRecord();
				foreach (var claim in principal.Claims)
				{
					switch (claim.Type)
					{
						case ApiConst.Claims.TokenId:
							claims.TrySetTokenId(claim.Value);
							break;
						case ApiConst.Claims.UserId:
							claims.TrySetUserId(claim.Value);
							break;
					}
				}

				return claims.IsValid();
			}
		}
		catch
		{
			// swallow
		}

		claims = default!;
		return false;
	}
}