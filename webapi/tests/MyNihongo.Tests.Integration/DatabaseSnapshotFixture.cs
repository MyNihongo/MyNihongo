namespace MyNihongo.Tests.Integration;

public abstract class DatabaseSnapshotFixture<T> : IDisposable
	where T : DatabaseFixture
{
	protected DatabaseSnapshotFixture(T databaseFixture)
	{
		DatabaseFixture = databaseFixture;
	}

	internal T DatabaseFixture { get; }

	public void Dispose()
	{
		DatabaseFixture.RestoreSnapshot();
		GC.SuppressFinalize(this);
	}
}