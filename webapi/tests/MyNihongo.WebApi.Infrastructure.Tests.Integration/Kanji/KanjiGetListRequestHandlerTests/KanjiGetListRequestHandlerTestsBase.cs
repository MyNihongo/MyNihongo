namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji.KanjiGetListRequestHandlerTests;

public abstract class KanjiGetListRequestHandlerTestsBase : KanjiIntegrationTestsBase
{
	protected KanjiGetListRequestHandlerTestsBase(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}

	internal KanjiGetListRequestHandler CreateFixture() =>
		new(new KanjiDatabaseService(Configuration));
}