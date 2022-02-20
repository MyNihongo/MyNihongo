namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji;

public abstract class KanjiIntegrationTestsBase : IntegrationTestsBase<KanjiDatabaseSnapshot, KanjiDatabaseFixture>
{
	protected KanjiIntegrationTestsBase(KanjiDatabaseSnapshot snapshot)
		: base(snapshot)
	{
	}
}