namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetListRequestHandlerTests;

[Collection(KanjiCollection.Name), UsesVerify]
public sealed class QueryByText : KanjiGetListRequestHandlerTestsBase
{
	public QueryByText(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	[Fact]
	public async Task QueryByLanguageAndRomaji()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "ba"
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task QueryByKana()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "ば"
		};

		var result1 = await CreateFixture()
			.HandleAsync(req);

		req.SearchText = "バ";

		var result2 = await CreateFixture()
			.HandleAsync(req);

		result1
			.Should()
			.BeEquivalentTo(result2);

		var result = result2.GetJson();
		await Verify(result);
	}

	[Fact]
	public async Task QueryByLanguage()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "été",
			Language = Language.French
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task FilterByJlptLevel()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "ho",
			JlptLevel = JlptLevel.N5
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task FilterByUserData()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "ya",
			Filter = KanjiGetListRequest.Types.Filter.UserData,
			UserId = 1L
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task FilterByUserDataAndJlptLevel()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "gan",
			JlptLevel = JlptLevel.N3,
			Filter = KanjiGetListRequest.Types.Filter.UserData,
			UserId = 2L
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task FilterByFavourites()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "opini",
			Filter = KanjiGetListRequest.Types.Filter.Favourites,
			UserId = 2L
		};

		var result = await CreateFixture()
			.HandleJsonAsync(req);

		await Verify(result);
	}

	[Fact]
	public async Task FilterByFavouritesAndJlptLevel()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "opini",
			JlptLevel = JlptLevel.N5,
			Filter = KanjiGetListRequest.Types.Filter.Favourites,
			UserId = 2L
		};

		var result = await CreateFixture()
			.HandleAsync(req);

		result
			.Should()
			.BeEmpty();
	}
}