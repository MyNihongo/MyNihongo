namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetListRequestHandlerTests;

[Collection(CollectionDefinitions.Kanji)]
public sealed class QueryByJlpt : KanjiGetListRequestHandlerTestsBase
{
	public QueryByJlpt(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	[Fact]
	public async Task QueryFirstPage()
	{
		var request = new KanjiGetListRequest
		{
			PageSize = 15
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QuerySecondPage()
	{
		var request = new KanjiGetListRequest
		{
			PageSize = 15,
			PageIndex = 1
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QueryFirstUser()
	{
		var request = new KanjiGetListRequest
		{
			UserId = 1L,
			PageSize = 10
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}

	[Fact]
	public async Task QuerySecondUser()
	{
		var request = new KanjiGetListRequest
		{
			UserId = 2L,
			PageSize = 10
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
			JlptLevel = JlptLevel.N3,
			PageSize = 15
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
			JlptLevel = JlptLevel.N5,
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
			JlptLevel = JlptLevel.N4,
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
			JlptLevel = JlptLevel.N3,
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
			JlptLevel = JlptLevel.N2,
			Language = Language.Portuguese
		};

		var result = await CreateFixture()
			.Handle(request, CancellationToken.None)
			.ToArrayAsync();

		ObjectApprovals.VerifyJson(result);
	}
}