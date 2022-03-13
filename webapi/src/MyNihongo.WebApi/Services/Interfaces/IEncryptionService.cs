namespace MyNihongo.WebApi.Services;

public interface IEncryptionService
{
	string Encrypt(string text, string passKey);

	string Decrypt(string text, string passKey);
}