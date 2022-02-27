namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetDetailRequestHandlerTests;

public abstract class KanjiGetDetailRequestHandlerTestsBase : KanjiIntegrationTestsBase
{
	protected KanjiGetDetailRequestHandlerTestsBase(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	internal KanjiGetDetailRequestHandler CreateFixture() =>
		new(new KanjiDatabaseService(Configuration));
}