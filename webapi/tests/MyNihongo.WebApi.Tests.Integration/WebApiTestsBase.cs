namespace MyNihongo.WebApi.Tests.Integration;

public abstract class WebApiTestsBase : IntegrationTestsBase<WebApiSnapshotFixture, WebApiFixture>
{
	protected WebApiTestsBase(WebApiSnapshotFixture snapshot)
		: base(snapshot)
	{
	}

	public async Task<object> GetConnectionDataAsync(long userId)
	{
		await using var connection = Snapshot.OpenConnection();

		return await connection.Connections
			.Where(x => x.UserId == userId)
			.Select(x => new { x.ConnectionId, x.UserId, x.IpAddress, x.ClientInfo })
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task VerifyTokensRegeneratedAsync(long userId)
	{
		await using var connection = Snapshot.OpenConnection();

		var tokens = await connection.Connections
			.Where(x => x.UserId == userId)
			.InnerJoin(
				connection.ConnectionTokens,
				(x, y) => x.ConnectionId == y.ConnectionId,
				(x, y) => y.TokenId)
			.ToArrayAsync()
			.ConfigureAwait(false);

		tokens
			.Should()
			.NotBeEmpty();

		foreach (var token in tokens)
		{
			token
				.Should()
				.NotBe(WebApiFixture.AccessId)
				.And
				.NotBe(WebApiFixture.RefreshId);
		}
	}
}