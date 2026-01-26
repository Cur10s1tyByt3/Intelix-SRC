using Microsoft.Win32;

namespace CvMega.Helper;

internal class RegeditKey
{
	public static string Regkey = "SOFTWARE\\Google\\CrashReports";

	public static bool CheckValue(string name)
	{
		using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
		{
			using RegistryKey registryKey2 = registryKey.CreateSubKey(Regkey, writable: false);
			if (registryKey2.GetValue(name) != null)
			{
				return true;
			}
		}
		return false;
	}

	public static void SetValue(string name, string value)
	{
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		using RegistryKey registryKey2 = registryKey.CreateSubKey(Regkey, writable: true);
		if (CheckValue(name))
		{
			registryKey2.DeleteValue(name);
		}
		registryKey2.SetValue(name, value);
	}

	public static string GetValue(string name)
	{
		if (!CheckValue(name))
		{
			return null;
		}
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		using RegistryKey registryKey2 = registryKey.CreateSubKey(Regkey, writable: false);
		return (string)registryKey2.GetValue(name);
	}

	public static string[] GetValues()
	{
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		using RegistryKey registryKey2 = registryKey.CreateSubKey(Regkey, writable: false);
		return registryKey2.GetValueNames();
	}

	public static void DeleteValue(string name)
	{
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		using RegistryKey registryKey2 = registryKey.CreateSubKey(Regkey, writable: true);
		if (CheckValue(name))
		{
			registryKey2.DeleteValue(name);
		}
	}
}
