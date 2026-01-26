using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;

namespace Intelix.Helper;

public static class AntiVirtual
{
	public static void CheckOrExit()
	{
		if (ProccessorCheck())
		{
			throw new Exception();
		}
		if (CheckDebugger())
		{
			throw new Exception();
		}
		if (CheckMemory())
		{
			throw new Exception();
		}
		if (CheckDriveSpace())
		{
			throw new Exception();
		}
		if (CheckUserConditions())
		{
			throw new Exception();
		}
		if (CheckCache())
		{
			throw new Exception();
		}
		if (CheckFileName())
		{
			throw new Exception();
		}
		if (CheckCim())
		{
			throw new Exception();
		}
	}

	public static bool CheckFileName()
	{
		return Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName).ToLower().Contains("sandbox");
	}

	public static bool ProccessorCheck()
	{
		return Environment.ProcessorCount <= 1;
	}

	public static bool CheckDebugger()
	{
		return Debugger.IsAttached;
	}

	public static bool CheckDriveSpace()
	{
		return new DriveInfo("C").TotalSize / 1073741824 < 50;
	}

	public static bool CheckCache()
	{
		return CheckCount("Select * from Win32_CacheMemory");
	}

	public static bool CheckCim()
	{
		return CheckCount("Select * from CIM_Memory");
	}

	public static bool CheckCount(string selector)
	{
		return new ManagementObjectSearcher(selector).Get().Count == 0;
	}

	public static bool CheckMemory()
	{
		return Convert.ToDouble(new ManagementObjectSearcher("Select * From Win32_ComputerSystem").Get().Cast<ManagementObject>().FirstOrDefault()["TotalPhysicalMemory"]) / 1048576.0 < 2048.0;
	}

	public static bool CheckUserConditions()
	{
		string text = Environment.UserName.ToLower();
		string text2 = Environment.MachineName.ToLower();
		if ((!(text == "frank") || !text2.Contains("desktop")) && !(text == "WDAGUtilityAccount"))
		{
			if (text == "robert")
			{
				return text2.Contains("22h2");
			}
			return false;
		}
		return true;
	}
}
