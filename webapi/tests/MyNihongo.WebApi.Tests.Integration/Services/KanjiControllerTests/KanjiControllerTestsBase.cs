namespace MyNihongo.WebApi.Tests.Integration.Services.KanjiControllerTests;

public abstract class KanjiControllerTestsBase : WebApiTestsBase
{
	protected KanjiControllerTestsBase(WebApiSnapshotFixture snapshot)
		: base(snapshot)
	{
	}

	protected KanjiClient CreateFixture() =>
		new(Snapshot.OpenChannel());
}