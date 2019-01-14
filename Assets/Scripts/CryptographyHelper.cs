/**
 * Created by: Victoria Shenkevich
 * Created on: 13/01/2019
 */

using System;
using System.Security.Cryptography;
using System.Text;

public static class CryptographyHelper
{
	private static string hash = "1234567890!@#$%^&*()";

	/// <summary>
	/// Returns encrypted string
	/// </summary>
	/// <param name="input">String to encrypt</param>
	/// <returns></returns>
	public static string Encrypt (string input)
	{
		byte[] inputAsBytes = UTF8Encoding.UTF8.GetBytes(input);

		using (MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider())
		{
			byte[] key = md5CryptoServiceProvider.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = 	new TripleDESCryptoServiceProvider()
																					{
																						Key = key,
																						Mode = CipherMode.ECB,
																						Padding = PaddingMode.PKCS7
																					})
			{
				ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
				byte[] resultAsBytes = cryptoTransform.TransformFinalBlock(inputAsBytes, 0, inputAsBytes.Length);

				return Convert.ToBase64String (resultAsBytes, 0, resultAsBytes.Length);
			}
		}
	}

	/// <summary>
	/// Returns decrypted string
	/// </summary>
	/// <param name="input">String to decrypt</param>
	/// <returns></returns>
	public static string Decrypt (string input)
	{
		byte[] inputAsBytes = Convert.FromBase64String (input);

		using (MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider())
		{
			byte[] key = md5CryptoServiceProvider.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = 	new TripleDESCryptoServiceProvider()
																					{
																						Key = key,
																						Mode = CipherMode.ECB,
																						Padding = PaddingMode.PKCS7
																					})
			{
				ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
				byte[] resultAsBytes = cryptoTransform.TransformFinalBlock(inputAsBytes, 0, inputAsBytes.Length);
				
				return UTF8Encoding.UTF8.GetString (resultAsBytes);
			}
		}
	}
}