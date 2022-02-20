namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji;

public sealed class KanjiDatabaseSnapshot : SnapshotClassFixture<KanjiDatabaseFixture>
{
	public KanjiDatabaseSnapshot(KanjiDatabaseFixture databaseCollectionFixture)
		: base(databaseCollectionFixture)
	{
	}
}