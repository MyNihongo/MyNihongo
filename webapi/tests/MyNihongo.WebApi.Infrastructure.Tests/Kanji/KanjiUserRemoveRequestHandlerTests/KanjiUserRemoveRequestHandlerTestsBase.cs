namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiUserRemoveRequestHandlerTests;

public abstract class KanjiUserRemoveRequestHandlerTestsBase : KanjiTestsBase
{
	internal KanjiUserRemoveRequestHandler CreateFixture() =>
		new(MockClock.Object, MockDatabase.Object);
}