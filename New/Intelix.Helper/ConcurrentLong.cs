using System.Threading;

namespace Intelix.Helper;

public struct ConcurrentLong
{
	private long _value;

	public long Value
	{
		get
		{
			return Interlocked.Read(ref _value);
		}
		set
		{
			Interlocked.Exchange(ref _value, value);
		}
	}

	public ConcurrentLong(long initial)
	{
		_value = initial;
	}

	public static ConcurrentLong operator ++(ConcurrentLong x)
	{
		Interlocked.Increment(ref x._value);
		return x;
	}

	public static ConcurrentLong operator --(ConcurrentLong x)
	{
		Interlocked.Decrement(ref x._value);
		return x;
	}

	public static implicit operator long(ConcurrentLong x)
	{
		return x.Value;
	}

	public static implicit operator ConcurrentLong(long v)
	{
		return new ConcurrentLong(v);
	}

	public static ConcurrentLong operator +(ConcurrentLong x, long y)
	{
		Interlocked.Add(ref x._value, y);
		return x;
	}

	public static ConcurrentLong operator -(ConcurrentLong x, long y)
	{
		Interlocked.Add(ref x._value, -y);
		return x;
	}
}
