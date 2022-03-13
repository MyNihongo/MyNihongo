using System.Security.Cryptography;
using System.Text;

namespace MyNihongo.WebApi.Services;

public sealed class EncryptionService : IEncryptionService
{
	private const int BlockSize = 128,
		KeySize = BlockSize / 8,
		DeviationIterationCount = 1000;

	private const CipherMode Cipher = CipherMode.CBC;
	private const PaddingMode Padding = PaddingMode.PKCS7;

	private static Encoding Encoding => Encoding.UTF8;

	public string Encrypt(string text, string passKey)
	{
		var saltStringBytes = Generate256BitsOfRandomEntropy();
		var ivStringBytes = Generate256BitsOfRandomEntropy();
		var textBytes = Encoding.GetBytes(text);

		var keyBytes = GetPassKeyBytes(passKey, saltStringBytes);
		using var rijndaelAlgorithm = GetRijndaelAlgorithm();

		using var encryptor = rijndaelAlgorithm.CreateEncryptor(keyBytes, ivStringBytes);
		using var memoryStream = new MemoryStream();
		using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

		cryptoStream.Write(textBytes, 0, textBytes.Length);
		cryptoStream.FlushFinalBlock();

		return CreateEncryptedText(ref saltStringBytes, ref ivStringBytes, memoryStream);
	}

	public string Decrypt(string text, string passKey)
	{
		var (saltBytes, ivBytes, textBytes) = ParseBytes(text);
		var keyBytes = GetPassKeyBytes(passKey, saltBytes);
		using var rijndaelAlgorithm = GetRijndaelAlgorithm();

		using var decryptor = rijndaelAlgorithm.CreateDecryptor(keyBytes, ivBytes);
		using var memoryStream = new MemoryStream(textBytes);
		using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

		int count;
		var stringBuilder = new StringBuilder();
		var buffer = new byte[textBytes.Length];

		var spanBuffer = buffer.AsSpan();
		while ((count = cryptoStream.Read(spanBuffer)) != 0)
			stringBuilder.Append(Encoding.GetString(buffer, 0, count));

		return stringBuilder.ToString();
	}

	private static byte[] GetPassKeyBytes(string passKey, byte[] saltStringBytes)
	{
		using var password = new Rfc2898DeriveBytes(passKey, saltStringBytes, DeviationIterationCount);
		return password.GetBytes(KeySize);
	}

	private static Aes GetRijndaelAlgorithm()
	{
		var aes = Aes.Create();
		aes.BlockSize = BlockSize;
		aes.Mode = Cipher;
		aes.Padding = Padding;

		return aes;
	}

	private static byte[] Generate256BitsOfRandomEntropy() =>
		RandomNumberGenerator.GetBytes(KeySize);

	private static string CreateEncryptedText(ref byte[] saltStringBytes, ref byte[] ivStringBytes, MemoryStream memoryStream)
	{
		var bytes = saltStringBytes
			.Concat(ivStringBytes)
			.Concat(memoryStream.ToArray())
			.ToArray();

		return Convert.ToBase64String(bytes);
	}

	private static BytesModel ParseBytes(string text)
	{
		var textWithSaltAndIv = Convert.FromBase64String(text);

		var saltBytes = textWithSaltAndIv
			.Take(KeySize)
			.ToArray();

		var ivBytes = textWithSaltAndIv
			.Skip(KeySize)
			.Take(KeySize)
			.ToArray();

		var textBytes = textWithSaltAndIv
			.Skip(KeySize * 2)
			.ToArray();

		return new BytesModel(saltBytes, ivBytes, textBytes);
	}

	private readonly ref struct BytesModel
	{
		public BytesModel(byte[] saltBytes, byte[] ivBytes, byte[] textBytes)
		{
			SaltBytes = saltBytes;
			IvBytes = ivBytes;
			TextBytes = textBytes;
		}

		public byte[] SaltBytes { get; }

		public byte[] IvBytes { get; }

		public byte[] TextBytes { get; }

		public void Deconstruct(out byte[] saltBytes, out byte[] ivBytes, out byte[] textBytes)
		{
			saltBytes = SaltBytes;
			ivBytes = IvBytes;
			textBytes = TextBytes;
		}
	}
}