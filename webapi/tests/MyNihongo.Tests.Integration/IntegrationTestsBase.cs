namespace MyNihongo.Tests.Integration;

public abstract class IntegrationTestsBase<TSnapshot, TConnection> : IClassFixture<TSnapshot>
	where TSnapshot : SnapshotClassFixture<TConnection>
	where TConnection : DatabaseCollectionFixture
{
	protected IntegrationTestsBase(TSnapshot snapshot)
	{
		Snapshot = snapshot;
	}

	public TSnapshot Snapshot { get; }

	public IConfiguration Configuration => Snapshot.Configuration;
}