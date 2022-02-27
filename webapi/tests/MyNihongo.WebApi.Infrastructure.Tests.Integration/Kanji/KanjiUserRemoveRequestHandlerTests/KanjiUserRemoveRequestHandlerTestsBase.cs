namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiUserRemoveRequestHandlerTests;

public abstract class KanjiUserRemoveRequestHandlerTestsBase : KanjiIntegrationTestsBase
{
	protected KanjiUserRemoveRequestHandlerTestsBase(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	protected Mock<IClock> MockClock { get; } = new();

	internal KanjiUserRemoveRequestHandler CreateFixture() =>
		new(MockClock.Object, new KanjiDatabaseService(Configuration));
}