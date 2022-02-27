namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiUserAddRequestHandlerTests;

[Collection(KanjiCollection.Name), UsesVerify]
public sealed class AddOrUpdate : KanjiUserAddRequestHandlerTestsBase
{
	private const long UserId = 1L;

	public AddOrUpdate(KanjiDatabaseSnapshot snapshot)
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
	public async Task AddNew()
	{
		MockClock.SetupInstant(2022, 2, 26, 12);

		var req = new KanjiUserAddRequest
		{
			KanjiId = 23,
			UserId = UserId
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task NotModifyAdded()
	{
		MockClock.SetupInstant(2022, 2, 26, 13);

		var req = new KanjiUserAddRequest
		{
			KanjiId = 11,
			UserId = UserId
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task ModifyDeleted()
	{
		MockClock.SetupInstant(2022, 2, 26, 14);

		var req = new KanjiUserAddRequest
		{
			KanjiId = 8,
			UserId = UserId
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
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
			.Where(x => x.UserId == UserId)
			.ToArrayAsync();
	}
}