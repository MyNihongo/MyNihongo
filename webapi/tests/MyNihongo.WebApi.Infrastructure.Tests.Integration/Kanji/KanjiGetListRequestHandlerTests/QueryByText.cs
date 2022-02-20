namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetListRequestHandlerTests;

[Collection(CollectionDefinitions.Kanji)]
public sealed class QueryByText : KanjiGetListRequestHandlerTestsBase
{
	public QueryByText(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	[Fact]
	public async Task QueryByLanguageAndRomaji()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "ba"
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QueryByKana()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "ば"
		};

		var result1 = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		request.SearchText = "バ";

		var result2 = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		result1
			.Should()
			.BeEquivalentTo(result2);

		ObjectApprovals.VerifyJson(result1);
	}

	[Fact]
	public async Task QueryByLanguage()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "été",
			Language = Language.French
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterByJlptLevel()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "ho",
			JlptLevel = JlptLevel.N5
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterByUserData()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "ya",
			Filter = KanjiGetListRequest.Types.Filter.UserData,
			UserId = 1L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterByUserDataAndJlptLevel()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "gan",
			JlptLevel = JlptLevel.N3,
			Filter = KanjiGetListRequest.Types.Filter.UserData,
			UserId = 2L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterByFavourites()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "opini",
			Filter = KanjiGetListRequest.Types.Filter.Favourites,
			UserId = 2L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterByFavouritesAndJlptLevel()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "opini",
			JlptLevel = JlptLevel.N5,
			Filter = KanjiGetListRequest.Types.Filter.Favourites,
			UserId = 2L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		result
			.Should()
			.BeEmpty();
	}
}