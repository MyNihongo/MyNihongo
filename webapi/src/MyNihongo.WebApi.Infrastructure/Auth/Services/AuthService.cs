namespace MyNihongo.WebApi.Infrastructure.Auth;

internal sealed class AuthService : IAuthService
{
	private readonly IAuthDatabaseService _authDatabaseService;
	private readonly IClock _clock;

	public AuthService(
		IAuthDatabaseService authDatabaseService,
		IClock clock)
	{
		_authDatabaseService = authDatabaseService;
		_clock = clock;
	}

	public async Task<Guid?> TrySetConnectionInfoAsync(AuthSetConnectionInfoParams parameters, CancellationToken ct = default)
	{
		var @params = new AuthConnectionSetParams
		{
			TokenId = parameters.TokenId,
			UserId = parameters.UserId,
			IpAddress = parameters.IpAddress,
			ClientInfo = parameters.ClientInfo,
			TicksNow = _clock.GetTicksNow()
		};

		var result = await _authDatabaseService.ConnectionSetAsync(@params, ct)
			.ConfigureAwait(false);

		return result?.ConnectionId;
	}

	public async Task<AuthRegenerateTokensResult> RegenerateTokensAsync(AuthRecreateTokensParams parameters, CancellationToken ct = default)
	{
		var result = await _authDatabaseService.RecreateTokensAsync(parameters, ct)
			.ConfigureAwait(false);

		return new AuthRegenerateTokensResult(result.AccessTokenId, result.RefreshTokenId);
	}
}