using System.Security.Cryptography;
using System.Text;

namespace Wilco.Security;

public static class Encryption
{
	private const int keysize = 256;

	// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
	// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
	private const string initVector = "T9sacho8i0iTrI4r";
	private const string passPhrase = "vayoru0ifraq2Bra";

	//Decrypt
	public static string DecryptString(string cipherText)
	{
		var password = new PasswordDeriveBytes(passPhrase, null);

		var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
		var cipherTextBytes = Convert.FromBase64String(cipherText);
		var keyBytes = password.GetBytes(keysize / 8);

		using var aes = Aes.Create("AesManaged")!;
		aes.Mode = CipherMode.CBC;
		var decryptor = aes.CreateDecryptor(keyBytes, initVectorBytes);
		var memoryStream = new MemoryStream(cipherTextBytes);
		var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
		var plainTextBytes = new byte[cipherTextBytes.Length];
		var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

		memoryStream.Close();
		cryptoStream.Close();

		return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
	}

	//Encrypt
	public static string EncryptString(string plainText)
	{
		var password = new PasswordDeriveBytes(passPhrase, null);

		var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
		var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
		var keyBytes = password.GetBytes(keysize / 8);

		using var aes = Aes.Create("AesManaged")!;
		aes.Mode = CipherMode.CBC;
		var encryptor = aes.CreateEncryptor(keyBytes, initVectorBytes);
		var memoryStream = new MemoryStream();
		var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
		cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
		cryptoStream.FlushFinalBlock();
		var cipherTextBytes = memoryStream.ToArray();

		memoryStream.Close();
		cryptoStream.Close();

		return Convert.ToBase64String(cipherTextBytes);
	}
}