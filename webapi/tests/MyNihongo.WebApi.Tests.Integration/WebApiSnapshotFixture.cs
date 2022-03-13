using Grpc.Net.Client;

namespace MyNihongo.WebApi.Tests.Integration;

public sealed class WebApiSnapshotFixture : DatabaseSnapshotFixture<WebApiFixture>
{
	public WebApiSnapshotFixture(WebApiFixture databaseCollectionFixture)
		: base(databaseCollectionFixture)
	{
	}

	public string User1AccessToken => WebApiFixture.User1AccessToken;
	
	public string User1RefreshToken => WebApiFixture.User1RefreshToken;

	public GrpcChannel OpenChannel() =>
		DatabaseFixture.OpenChannel();
}