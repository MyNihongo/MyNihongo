namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetDetailRequestHandlerTests;

[Collection(KanjiCollection.Name), UsesVerify]
public sealed class QueryDetail : KanjiGetDetailRequestHandlerTestsBase
{
	public QueryDetail(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	[Fact]
	public async Task Anonymous()
	{
		var req = new KanjiGetDetailRequest
		{
			KanjiId = 14
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task UserNoData()
	{
		var req = new KanjiGetDetailRequest
		{
			KanjiId = 10,
			UserId = 1L
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task UserData()
	{
		var req = new KanjiGetDetailRequest
		{
			KanjiId = 11,
			UserId = 1L
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task UserDataDeleted()
	{
		var req = new KanjiGetDetailRequest
		{
			KanjiId = 10,
			UserId = 2L
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}
}