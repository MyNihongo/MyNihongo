namespace MyNihongo.Tests.Integration;

public abstract class DatabaseSnapshotFixture<T> : IDisposable
	where T : DatabaseFixture
{
	protected DatabaseSnapshotFixture(T databaseFixture)
	{
		DatabaseFixture = databaseFixture;
	}

	protected internal T DatabaseFixture { get; }

	public DatabaseConnection OpenConnection() =>
		DatabaseFixture.OpenConnection();

	public void Dispose()
	{
		DatabaseFixture.RestoreSnapshot();
		GC.SuppressFinalize(this);
	}
}