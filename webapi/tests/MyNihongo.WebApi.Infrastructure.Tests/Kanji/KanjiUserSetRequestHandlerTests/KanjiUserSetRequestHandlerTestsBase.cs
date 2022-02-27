namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiUserSetRequestHandlerTests;

public abstract class KanjiUserSetRequestHandlerTestsBase : KanjiTestsBase
{
	internal KanjiUserSetRequestHandler CreateFixture() =>
		new(MockClock.Object, MockDatabase.Object);
}