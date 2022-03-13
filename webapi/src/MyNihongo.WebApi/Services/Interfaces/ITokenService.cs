using MyNihongo.WebApi.Models.Auth;

namespace MyNihongo.WebApi.Services;

public interface ITokenService
{
	Task<AuthGenerateTokensResult> RegenerateTokensAsync(AuthConnectionValidationResult validationResult, CancellationToken ct = default);

	ValueTask<long?> ValidateAccessTokenAsync(AuthConnectionValidationParams parameters, CancellationToken ct = default);

	ValueTask<AuthConnectionValidationResult?> ValidateRefreshTokenAsync(AuthConnectionValidationParams parameters, CancellationToken ct = default);
}