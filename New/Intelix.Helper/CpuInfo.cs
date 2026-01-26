using System;
using Microsoft.Win32;

namespace Intelix.Helper;

public static class CpuInfo
{
	public static string GetName()
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
			return (registryKey?.GetValue("ProcessorNameString") as string) ?? (registryKey?.GetValue("VendorIdentifier") as string) ?? "Unknown";
		}
		catch
		{
			return "Unknown";
		}
	}

	public static int GetLogicalCores()
	{
		try
		{
			return Environment.ProcessorCount;
		}
		catch
		{
			return 0;
		}
	}
}
