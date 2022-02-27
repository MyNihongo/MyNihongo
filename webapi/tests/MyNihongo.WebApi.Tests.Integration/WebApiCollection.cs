namespace MyNihongo.WebApi.Tests.Integration;

[CollectionDefinition(Name)]
public sealed class WebApiCollection : ICollectionFixture<WebApiFixture>
{
	public const string Name = "WebApi";
}