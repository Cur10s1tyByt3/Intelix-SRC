using System;
using System.Text;

namespace CvMega.Helper;

internal class RandomPathGenerator
{
	private static readonly Random random = new Random();

	private static readonly string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	private static readonly string[] extensions = new string[7] { "", ".txt", ".png", ".jpg", ".html", ".json", "" };

	public static string GenerateCompletelyRandomPath()
	{
		int num = random.Next(1, 11);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < num; i++)
		{
			int length = random.Next(3, 20);
			stringBuilder.Append('/');
			stringBuilder.Append(GenerateRandomString(length));
		}
		if (random.Next(2) == 1)
		{
			stringBuilder.Append(extensions[random.Next(extensions.Length)]);
		}
		return stringBuilder.ToString();
	}

	private static string GenerateRandomString(int length)
	{
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append(chars[random.Next(chars.Length)]);
		}
		return stringBuilder.ToString();
	}
}
