namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiUserAddRequestHandlerTests;

public abstract class KanjiUserAddRequestHandlerTestsBase : KanjiTestsBase
{
	protected KanjiUserAddRequestHandlerTestsBase()
	{
		MockDatabase
			.Setup(x => x.UserAddAsync(It.IsAny<KanjiUserAddParams>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new KanjiUserAddResult());
	}

	internal KanjiUserAddRequestHandler CreateFixture() =>
		new(MockClock.Object, MockDatabase.Object);
}