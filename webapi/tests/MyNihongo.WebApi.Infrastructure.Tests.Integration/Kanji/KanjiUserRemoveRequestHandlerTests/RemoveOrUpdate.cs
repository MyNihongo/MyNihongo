namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiUserRemoveRequestHandlerTests;

[Collection(KanjiCollection.Name), UsesVerify]
public sealed class RemoveOrUpdate : KanjiUserRemoveRequestHandlerTestsBase
{
	private const long UserId = 1L;
	private const short AnotherUserKanjiId = 9;

	public RemoveOrUpdate(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	[Order(int.MinValue), Fact]
	public async Task Before()
	{
		var data = await GetVerifyDataAsync()
			.GetJsonAsync();

		await Verify(data);
	}

	[Fact]
	public async Task RemoveAdded()
	{
		MockClock.SetupInstant(2022, 2, 26, 12);

		var req = new KanjiUserRemoveRequest
		{
			KanjiId = 11,
			UserId = UserId
		};

		await CreateFixture()
			.HandleAsync(req);
	}

	[Fact]
	public async Task NotRemoveRemoved()
	{
		MockClock.SetupInstant(2022, 2, 26, 13);

		var req = new KanjiUserRemoveRequest
		{
			KanjiId = 8,
			UserId = UserId
		};

		await CreateFixture()
			.HandleAsync(req);
	}

	[Fact]
	public async Task NotRemoveIfNotExists()
	{
		MockClock.SetupInstant(2022, 2, 26, 14);

		var req = new KanjiUserRemoveRequest
		{
			KanjiId = AnotherUserKanjiId,
			UserId = UserId
		};

		await CreateFixture()
			.HandleAsync(req);
	}

	[Order(int.MaxValue), Fact]
	public async Task After()
	{
		var data = await GetVerifyDataAsync()
			.GetJsonAsync();

		await Verify(data);
	}

	private async Task<object> GetVerifyDataAsync()
	{
		await using var connection = OpenConnection();

		return await connection.KanjiUserEntries
			.Where(x => x.UserId == UserId || x.KanjiId == AnotherUserKanjiId)
			.ToArrayAsync();
	}
}