namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiGetDetailRequestHandlerTests;

public abstract class KanjiGetDetailRequestHandlerTestsBase : KanjiTestsBase
{
	protected KanjiGetDetailRequestHandlerTestsBase()
	{
		MockDatabase
			.Setup(x => x.QueryDetailAsync(It.IsAny<KanjiGetDetailParams>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new KanjiGetDetailResult());
	}

	internal KanjiGetDetailRequestHandler CreateFixture() =>
		new(MockDatabase.Object);
}