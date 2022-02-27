namespace MyNihongo.WebApi.Tests.Integration;

public sealed class WebApiSnapshotFixture : DatabaseSnapshotFixture<WebApiFixture>
{
	public WebApiSnapshotFixture(WebApiFixture databaseCollectionFixture)
		: base(databaseCollectionFixture)
	{
	}
}