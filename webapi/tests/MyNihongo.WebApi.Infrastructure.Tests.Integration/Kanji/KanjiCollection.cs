namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji;

[CollectionDefinition(Name)]
public sealed class KanjiCollection : ICollectionFixture<KanjiDatabase>
{
	public const string Name = "Kanji";
}