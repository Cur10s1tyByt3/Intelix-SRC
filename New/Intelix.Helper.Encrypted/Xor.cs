using System.Text;

namespace Intelix.Helper.Encrypted;

public static class Xor
{
	public static string DecryptString(string input, byte key)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] ^= key;
		}
		return Encoding.UTF8.GetString(bytes);
	}
}
