namespace MyNihongo.WebApi.Models.Auth;

public sealed record AuthGenerateTokensResult(AuthGenerateTokensResult.TokenData? AccessToken, AuthGenerateTokensResult.TokenData? RefreshToken)
{
	private static readonly TokenData Empty = new(string.Empty, TimeSpan.Zero);

	public static readonly AuthGenerateTokensResult EmptyAccess = new(Empty, null),
		EmptyRefresh = new(null, Empty);

	public sealed record TokenData(string Value, TimeSpan Expires);
}
