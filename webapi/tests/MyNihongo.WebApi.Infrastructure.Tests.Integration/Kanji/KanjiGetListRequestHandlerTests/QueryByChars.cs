namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetListRequestHandlerTests;

[Collection(CollectionDefinitions.Kanji)]
public sealed class QueryByChars : KanjiGetListRequestHandlerTestsBase
{
	public QueryByChars(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	[Fact]
	public async Task QueryKanjiIgnoreText()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "夏romaji飲ひらがな願カタカナ餓 針"
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QueryUser()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "休説歩",
			UserId = 1L
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
			SearchText = "説八歩",
			JlptLevel = JlptLevel.N5
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterUserEntries()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "飲夏餓説亜",
			Filter = KanjiGetListRequest.Types.Filter.UserData,
			UserId = 1L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterUserEntriesAndJlptLevel()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "飲夏餓説亜休八",
			JlptLevel = JlptLevel.N4,
			Filter = KanjiGetListRequest.Types.Filter.UserData,
			UserId = 1L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterFavourites()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "元夏説飲癖",
			Filter = KanjiGetListRequest.Types.Filter.Favourites,
			UserId = 2L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task FilterFavouritesAndJlptLevel()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "元夏説亜飲願癖",
			JlptLevel = JlptLevel.N4,
			Filter = KanjiGetListRequest.Types.Filter.Favourites,
			UserId = 2L
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QueryFrench()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "個夏",
			Language = Language.French
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QuerySpanish()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "個夏",
			Language = Language.Spanish
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QueryPortuguese()
	{
		var request = new KanjiGetListRequest
		{
			SearchText = "個夏",
			Language = Language.Portuguese
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}
}