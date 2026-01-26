using System;
using System.Linq;

namespace Intelix.Helper;

public static class RandomStrings
{
	private const string Ascii = "abcdefghijklmnopqrstuvwxyz";

	private static readonly Random Random = new Random();

	public static string GenerateHashTag()
	{
		return " #" + GenerateString();
	}

	public static string GenerateString()
	{
		return GenerateString(5);
	}

	public static string GenerateString(int length)
	{
		char c = "abcdefghijklmnopqrstuvwxyz"[Random.Next("abcdefghijklmnopqrstuvwxyz".Length)];
		char[] value = (from s in Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", length - 1)
			select s[Random.Next(s.Length)]).ToArray();
		return c + new string(value);
	}
}
