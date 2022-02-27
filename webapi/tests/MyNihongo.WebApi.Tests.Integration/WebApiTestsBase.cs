namespace MyNihongo.WebApi.Tests.Integration;

public abstract class WebApiTestsBase : IntegrationTestsBase<WebApiSnapshotFixture, WebApiFixture>
{
	protected WebApiTestsBase(WebApiSnapshotFixture snapshot)
		: base(snapshot)
	{
	}
}