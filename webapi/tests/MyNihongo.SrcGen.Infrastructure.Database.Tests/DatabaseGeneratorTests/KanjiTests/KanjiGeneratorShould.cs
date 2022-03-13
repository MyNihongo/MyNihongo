namespace MyNihongo.SrcGen.Infrastructure.Database.Tests.DatabaseGeneratorTests.KanjiTests;

public sealed class KanjiGeneratorShould : DatabaseGeneratorTestsBase
{
	[Fact]
	public async Task GenerateKanjiService()
	{
		await VerifyGeneratorAsync(GeneratorKey.Kanji);
	}
}