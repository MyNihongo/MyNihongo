// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiGetDetailRequestHandlerTests;

public sealed class HandleShould : KanjiGetDetailRequestHandlerTestsBase
{
	[Fact]
	public async Task InvokeGetDetail()
	{
		const int kanjiId = 123;

		var expected = new KanjiGetDetailParams
		{
			KanjiId = kanjiId,
			Language = Language.English,
			UserId = null
		};

		var req = new KanjiGetDetailRequest
		{
			KanjiId = kanjiId
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.Handle(req, cts.Token);

		MockDatabase.Verify(x => x.QueryDetailAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}