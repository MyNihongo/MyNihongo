namespace MyNihongo.SrcGen.Infrastructure.Database.Tests.DatabaseGeneratorTests.AuthTests;

public sealed class AuthGeneratorShould : DatabaseGeneratorTestsBase
{
	[Fact]
	public async Task GenerateAuthService()
	{
		await VerifyGeneratorAsync(GeneratorKey.Auth);
	}
}