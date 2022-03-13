namespace MyNihongo.WebApi.Infrastructure.Auth;

internal interface IAuthDatabaseService
{
	/// <summary>
	/// Executes `spAuthConnectionSet` with the specified <see cref="parameters"/>
	/// </summary>
	Task<AuthConnectionSetResult?> ConnectionSetAsync(AuthConnectionSetParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spAuthRecreateTokens` with the specified <see cref="parameters"/>
	/// </summary>
	Task<AuthRecreateTokensResult> RecreateTokensAsync(AuthRecreateTokensParams parameters, CancellationToken ct = default);
}

internal sealed class AuthDatabaseService : IAuthDatabaseService
{
	private readonly IConfiguration _configuration;

	public AuthDatabaseService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<AuthConnectionSetResult?> ConnectionSetAsync(AuthConnectionSetParams parameters, CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Auth));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand("spAuthConnectionSet", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue("tokenID", parameters.TokenId);
		command.Parameters.AddWithValue("userID", parameters.UserId);
		command.Parameters.AddWithValue("ipAddress", parameters.IpAddress);
		command.Parameters.AddWithValue("clientInfo", parameters.ClientInfo);
		command.Parameters.AddWithValue("tickNow", parameters.TicksNow);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			return new AuthConnectionSetResult
			{
				ConnectionId = reader.GetGuid(0)
			};
		}

		return null;
	}

	public async Task<AuthRecreateTokensResult> RecreateTokensAsync(AuthRecreateTokensParams parameters, CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Auth));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand("spAuthRecreateTokens", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue("connectionID", parameters.ConnectionId);
		command.Parameters.AddWithValue("ticksAccessValidTo", parameters.TicksAccessValidTo);
		command.Parameters.AddWithValue("ticksRefreshValidTo", parameters.TicksRefreshValidTo);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			return new AuthRecreateTokensResult
			{
				AccessTokenId = reader.GetGuid(0),
				RefreshTokenId = reader.GetGuid(1)
			};
		}

		return new AuthRecreateTokensResult();
	}
}
