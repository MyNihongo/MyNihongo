namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiUserSetRequestHandlerTests;

[Collection(KanjiCollection.Name), UsesVerify]
public sealed class Set : KanjiUserSetRequestHandlerTestsBase
{
	private const long UserId = 1L;
	private const short AnotherUserKanjiId = 9;

	public Set(KanjiDatabaseSnapshot snapshot)
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
	public async Task SetAll()
	{
		MockClock.SetupInstant(2022, 2, 27, 12);

		var req = new KanjiUserSetRequest
		{
			KanjiId = 5,
			UserId = 1L,
			FavouriteRating = 3.5d,
			Notes = "new notes",
			Mark = 123
		};

		await CreateFixture()
			.HandleAsync(req);
	}

	[Fact]
	public async Task SetPartial()
	{
		MockClock.SetupInstant(2022, 2, 27, 13);

		var req = new KanjiUserSetRequest
		{
			KanjiId = 11,
			UserId = 1L,
			Notes = string.Empty,
			Mark = 31
		};

		await CreateFixture()
			.HandleAsync(req);
	}

	[Fact]
	public async Task NotSetRemoved()
	{
		MockClock.SetupInstant(2022, 2, 27, 14);

		var req = new KanjiUserSetRequest
		{
			KanjiId = 8,
			UserId = 1L,
			Notes = "notes for deleting"
		};

		await CreateFixture()
			.HandleAsync(req);
	}

	[Fact]
	public async Task NotSetAnotherUser()
	{
		MockClock.SetupInstant(2022, 2, 27, 15);

		var req = new KanjiUserSetRequest
		{
			KanjiId = AnotherUserKanjiId,
			UserId = 1L,
			Notes = "another user"
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