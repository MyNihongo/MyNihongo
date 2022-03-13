using Grpc.Core.Interceptors;
using MyNihongo.WebApi.Auth;
using MyNihongo.WebApi.Models.Auth;
using MyNihongo.WebApi.Resources.Const;

namespace MyNihongo.WebApi.Interceptors;

public sealed class JwtBearerInterceptor : Interceptor
{
	private readonly IUserSession _userSession;

	public JwtBearerInterceptor(IUserSession userSession)
	{
		_userSession = userSession;
	}

	public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
	{
		var res = await base.UnaryServerHandler(request, context, continuation)
			.ConfigureAwait(false);

		if (_userSession.TryGetTokenData(out var tokenData))
			SetTokenCookies(context, tokenData);

		return res;
	}

	public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
	{
		await base.ServerStreamingServerHandler(request, responseStream, context, continuation)
			.ConfigureAwait(false);

		if (_userSession.TryGetTokenData(out var tokenData))
			SetTokenCookies(context, tokenData);
	}

	private static void SetTokenCookies(in ServerCallContext ctx, in AuthGenerateTokensResult tokenData)
	{
		if (tokenData.AccessToken != null)
			SetCookie(ctx, ApiConst.Cookies.AccessToken, tokenData.AccessToken);

		if (tokenData.RefreshToken != null)
			SetCookie(ctx, ApiConst.Cookies.RefreshToken, tokenData.RefreshToken);

		static void SetCookie(in ServerCallContext ctx, in string cookieName, in AuthGenerateTokensResult.TokenData tokenData)
		{
			ctx.ResponseTrailers.Add("Set-Cookie", $"{cookieName}={tokenData.Value}; HttpOnly; SameSite=None; Secure; Max-Age={tokenData.Expires.TotalSeconds}");
		}
	}
}