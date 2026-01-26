using System.Net;

namespace Intelix.Helper;

public static class IpApi
{
	private static string _cachedIp;

	private static readonly object _lock = new object();

	public static string GetPublicIp()
	{
		if (!string.IsNullOrEmpty(_cachedIp))
		{
			return _cachedIp;
		}
		lock (_lock)
		{
			if (!string.IsNullOrEmpty(_cachedIp))
			{
				return _cachedIp;
			}
			try
			{
				using WebClient webClient = new WebClient();
				string text = webClient.DownloadString("http://icanhazip.com");
				if (!string.IsNullOrEmpty(text))
				{
					_cachedIp = text.Trim();
				}
			}
			catch
			{
				_cachedIp = "Request failed";
			}
			return _cachedIp;
		}
	}
}
