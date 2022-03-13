namespace MyNihongo.WebApi.Tests.Services.EncryptionServiceTests;

public sealed class DecryptShould : EncryptionServiceTestsBase
{
	[Fact]
	public void DecryptFromEncrypt()
	{
		const string text = nameof(text), passKey = nameof(passKey);
		var fixture = CreateFixture();

		var encrypted = fixture.Encrypt(text, passKey);
		var result = fixture.Decrypt(encrypted, passKey);

		result
			.Should()
			.Be(text);
	}

	[Fact]
	public void NotDecryptFromEncrypt()
	{
		const string text = nameof(text);
		var fixture = CreateFixture();

		var encrypted = fixture.Encrypt(text, "pass key");
		Action action = () => fixture.Decrypt(encrypted, "another");

		action
			.Should()
			.Throw<CryptographicException>();
	}
}