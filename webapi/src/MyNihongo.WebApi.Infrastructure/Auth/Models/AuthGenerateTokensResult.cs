namespace MyNihongo.WebApi.Infrastructure.Auth;

public sealed record AuthRegenerateTokensResult(Guid AccessTokenId, Guid RefreshTokenId);
