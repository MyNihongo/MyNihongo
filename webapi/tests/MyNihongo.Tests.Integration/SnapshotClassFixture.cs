namespace MyNihongo.Tests.Integration;

public abstract class SnapshotClassFixture<T> : IDisposable
	where T : DatabaseCollectionFixture
{
	private readonly T _databaseCollectionFixture;

	protected SnapshotClassFixture(T databaseCollectionFixture)
	{
		_databaseCollectionFixture = databaseCollectionFixture;
	}

	internal IConfiguration Configuration => _databaseCollectionFixture.Configuration;

	public void Dispose()
	{
		_databaseCollectionFixture.RestoreSnapshot();
		GC.SuppressFinalize(this);
	}
}