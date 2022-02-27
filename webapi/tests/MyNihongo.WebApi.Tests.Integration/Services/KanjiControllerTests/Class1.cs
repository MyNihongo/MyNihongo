namespace MyNihongo.WebApi.Tests.Integration.Services.KanjiControllerTests;

[Collection(WebApiCollection.Name), UsesVerify]
public sealed class Class1 : KanjiControllerTestsBase
{
	public Class1(WebApiSnapshotFixture snapshot)
		: base(snapshot)
	{
	}

	[Fact]
	public async Task Run()
	{
		// https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/grpc/test-services/sample/Tests/Server/IntegrationTests/IntegrationTestBase.cs
		// https://docs.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-6.0&tabs=visual-studio
		// https://docs.microsoft.com/en-us/aspnet/core/grpc/test-services?view=aspnetcore-6.0
	}
}