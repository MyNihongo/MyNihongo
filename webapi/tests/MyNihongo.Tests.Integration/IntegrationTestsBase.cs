namespace MyNihongo.Tests.Integration;

public abstract class IntegrationTestsBase<TSnapshot, TConnection> : IClassFixture<TSnapshot>
	where TSnapshot : DatabaseSnapshotFixture<TConnection>
	where TConnection : DatabaseFixture
{
	protected IntegrationTestsBase(TSnapshot snapshot)
	{
		Snapshot = snapshot;
	}

	public TSnapshot Snapshot { get; }

	public IConfiguration Configuration => Snapshot.DatabaseFixture.Configuration;

	public DatabaseConnection OpenConnection() =>
		Snapshot.DatabaseFixture.OpenConnection();
}