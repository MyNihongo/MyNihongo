namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiUserSetRequestHandlerTests;

public abstract class KanjiUserSetRequestHandlerTestsBase : KanjiIntegrationTestsBase
{
	protected KanjiUserSetRequestHandlerTestsBase(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	protected Mock<IClock> MockClock { get; } = new();

	internal KanjiUserSetRequestHandler CreateFixture() =>
		new(MockClock.Object, new KanjiDatabaseService(Configuration));
}