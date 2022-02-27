// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiUserSetRequestHandlerTests;

public sealed class HandleShould : KanjiUserSetRequestHandlerTestsBase
{
	[Fact]
	public async Task InvokeSetForAll()
	{
		const long userId = 321L, ticks = 16459740000000000L;
		const int kanjiId = 123;
		const double rating = 3.5d;
		const string notes = nameof(notes);
		const byte mark = 12;

		var expected = new KanjiUserSetParams
		{
			UserId = userId,
			KanjiId = kanjiId,
			FavouriteRating = 35,
			Notes = notes,
			Mark = mark,
			TicksModified = ticks
		};

		var req = new KanjiUserSetRequest
		{
			UserId = userId,
			KanjiId = kanjiId,
			FavouriteRating = rating,
			Notes = notes,
			Mark = mark
		};

		MockClock.SetupInstant(2022, 2, 27, 15);

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		MockDatabase.Verify(x => x.UserSetAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task InvokeSetForPartial()
	{
		const long userId = 321L, ticks = 16459740000000000L;
		const int kanjiId = 123;
		const double rating = 3.5d;
		const byte mark = 12;

		var expected = new KanjiUserSetParams
		{
			UserId = userId,
			KanjiId = kanjiId,
			FavouriteRating = 35,
			Mark = mark,
			TicksModified = ticks
		};

		var req = new KanjiUserSetRequest
		{
			UserId = userId,
			KanjiId = kanjiId,
			FavouriteRating = rating,
			Mark = mark
		};

		MockClock.SetupInstant(2022, 2, 27, 15);

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		MockDatabase.Verify(x => x.UserSetAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task NotInvokeIfAllWithoutValues()
	{
		const long userId = 321L;
		const int kanjiId = 123;

		var req = new KanjiUserSetRequest
		{
			UserId = userId,
			KanjiId = kanjiId
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		VerifyNoOtherCalls();
	}

	[Theory]
	[InlineData(-0.1d)]
	[InlineData(5.01d)]
	public async Task NotInvokeIfRatingOutsideBounds(double rating)
	{
		const long userId = 321L;
		const int kanjiId = 123;

		var req = new KanjiUserSetRequest
		{
			UserId = userId,
			KanjiId = kanjiId,
			FavouriteRating = rating
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		VerifyNoOtherCalls();
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(256)]
	public async Task NotInvokeIfMarkNotByte(int mark)
	{
		const long userId = 321L;
		const int kanjiId = 123;

		var req = new KanjiUserSetRequest
		{
			UserId = userId,
			KanjiId = kanjiId,
			Mark = mark
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		VerifyNoOtherCalls();
	}
}