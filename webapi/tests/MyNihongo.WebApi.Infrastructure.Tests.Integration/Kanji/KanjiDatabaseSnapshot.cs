namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji;

public sealed class KanjiDatabaseSnapshot : DatabaseSnapshotFixture<KanjiDatabase>
{
	public KanjiDatabaseSnapshot(KanjiDatabase databaseCollectionFixture)
		: base(databaseCollectionFixture)
	{
	}
}