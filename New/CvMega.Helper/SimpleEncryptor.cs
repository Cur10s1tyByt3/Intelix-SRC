using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CvMega.Helper;

public class SimpleEncryptor
{
	public static readonly string secretKey = "cvls0";

	public static string Encrypt(string data)
	{
		return HttpUtility.UrlEncode(Convert.ToBase64String(XorEncrypt(Encoding.UTF8.GetBytes(data))));
	}

	public static string EncryptNonEnc(string data)
	{
		return Convert.ToBase64String(XorEncrypt(Encoding.UTF8.GetBytes(data)));
	}

	public static string Decrypt(string data)
	{
		return Encoding.UTF8.GetString(XorEncrypt(Convert.FromBase64String(data)));
	}

	public static string Hash(string data)
	{
		using MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(data));
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return HttpUtility.UrlEncode(stringBuilder.ToString().Substring(0, 10));
	}

	public static byte[] XorEncryptMyKey(byte[] data, byte key)
	{
		byte[] array = new byte[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			array[i] = (byte)(data[i] ^ key);
		}
		return array;
	}

	public static byte[] XorEncrypt(byte[] dataBytes)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(secretKey);
		for (int i = 0; i < dataBytes.Length; i++)
		{
			dataBytes[i] ^= bytes[i % bytes.Length];
		}
		return dataBytes;
	}
}
