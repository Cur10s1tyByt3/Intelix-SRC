using Intelix.Helper.Data;

namespace Intelix.Targets;

public interface ITarget
{
	void Collect(InMemoryZip zip, Counter counter);
}
