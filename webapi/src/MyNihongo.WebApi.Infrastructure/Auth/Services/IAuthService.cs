namespace MyNihongo.WebApi.Infrastructure.Auth;

public interface IAuthService
{
	/// <returns>Connection ID</returns>
	Task<Guid?> TrySetConnectionInfoAsync(AuthSetConnectionInfoParams parameters, CancellationToken ct = default);

	Task<AuthRegenerateTokensResult> RegenerateTokensAsync(AuthRecreateTokensParams parameters, CancellationToken ct = default);
}