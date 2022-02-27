// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiUserRemoveRequestHandlerTests;

public sealed class HandleShould : KanjiUserRemoveRequestHandlerTestsBase
{
	[Fact]
	public async Task InvokeUserRemove()
	{
		const long userId = 321L, ticks = 16459740000000000L;
		const int kanjiId = 123;

		var expected = new KanjiUserRemoveParams
		{
			UserId = userId,
			KanjiId = kanjiId,
			TicksModified = ticks
		};

		var req = new KanjiUserRemoveRequest
		{
			UserId = userId,
			KanjiId = kanjiId
		};

		MockClock.SetupInstant(2022, 2, 27, 15);

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		MockDatabase.Verify(x => x.UserRemoveAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}