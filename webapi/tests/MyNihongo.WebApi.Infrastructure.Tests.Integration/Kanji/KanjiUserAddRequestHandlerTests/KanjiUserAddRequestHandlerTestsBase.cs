namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiUserAddRequestHandlerTests;

public abstract class KanjiUserAddRequestHandlerTestsBase : KanjiIntegrationTestsBase
{
	protected KanjiUserAddRequestHandlerTestsBase(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	protected Mock<IClock> MockClock { get; } = new();

	internal KanjiUserAddRequestHandler CreateFixture() =>
		new(MockClock.Object, new KanjiDatabaseService(Configuration));
}