namespace MyNihongo.WebApi.Infrastructure.Auth;

internal sealed record AuthRecreateTokensResult
{
	public Guid AccessTokenId { get; init; }

	public Guid RefreshTokenId { get; init; }
}