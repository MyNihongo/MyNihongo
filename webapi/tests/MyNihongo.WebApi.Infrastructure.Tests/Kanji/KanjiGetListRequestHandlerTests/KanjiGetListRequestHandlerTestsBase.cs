namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiGetListRequestHandlerTests;

public abstract class KanjiGetListRequestHandlerTestsBase : KanjiTestsBase
{
	protected KanjiGetListRequestHandlerTestsBase()
	{
		MockDatabase
			.Setup(x => x.QueryByJlptAsync(It.IsAny<KanjiGetListByJlptParams>(), It.IsAny<CancellationToken>()))
			.Returns(AsyncEnumerable.Empty<KanjiGetListResult>());

		MockDatabase
			.Setup(x => x.QueryByCharAsync(It.IsAny<KanjiGetListByCharParams>(), It.IsAny<CancellationToken>()))
			.Returns(AsyncEnumerable.Empty<KanjiGetListResult>());

		MockDatabase
			.Setup(x => x.QueryByTextAsync(It.IsAny<KanjiGetListByTextParams>(), It.IsAny<CancellationToken>()))
			.Returns(AsyncEnumerable.Empty<KanjiGetListResult>());
	}

	internal KanjiGetListRequestHandler CreateFixture() =>
		new(MockDatabase.Object);
}