// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiUserAddRequestHandlerTests;

public sealed class HandleShould : KanjiUserAddRequestHandlerTestsBase
{
	[Fact]
	public async Task InvokeUserAdd()
	{
		const long userId = 321L, ticks = 16459740000000000L;
		const int kanjiId = 123;

		var expected = new KanjiUserAddParams
		{
			UserId = userId,
			KanjiId = kanjiId,
			TicksModified = ticks
		};

		var req = new KanjiUserAddRequest
		{
			UserId = userId,
			KanjiId = kanjiId
		};

		MockClock.SetupInstant(2022, 2, 27, 15);

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		MockDatabase.Verify(x => x.UserAddAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}