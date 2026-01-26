using System.Threading;

namespace Intelix.Helper;

public static class MutexControl
{
	public static Mutex currentApp;

	public static bool createdNew;

	public static bool CreateMutex(string mtx)
	{
		currentApp = new Mutex(initiallyOwned: false, mtx, out createdNew);
		return createdNew;
	}
}
