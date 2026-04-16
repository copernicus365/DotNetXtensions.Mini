namespace DotNetXtensions;

public static partial class XDateTimes
{
	// http://stackoverflow.com/questions/7029353/how-can-i-round-up-the-time-to-the-nearest-x-minutes

	// --- Round ---

	/// <summary>
	/// Rounds the DateTime to the nearest specified interval.
	/// Values at the exact midpoint are rounded up.
	/// <para/>
	/// Thanks to DevSal on http://stackoverflow.com/questions/7029353/c-sharp-round-up-time-to-nearest-x-minutes.
	/// </summary>
	/// <param name="dt">DateTime to round.</param>
	/// <param name="roundBy">TimeSpan to round to.</param>
	public static DateTime Round(this DateTime dt, TimeSpan roundBy)
	{
		bool roundUp = _roundUp(dt.Ticks, roundBy);
		return roundUp ? dt.RoundUp(roundBy) : dt.RoundDown(roundBy);
	}

	/// <summary>
	/// Rounds the DateTimeOffset to the nearest specified interval.
	/// Values at the exact midpoint are rounded up.
	/// <para/>
	/// Thanks to DevSal on http://stackoverflow.com/questions/7029353/c-sharp-round-up-time-to-nearest-x-minutes.
	/// </summary>
	/// <param name="dt">DateTimeOffset to round.</param>
	/// <param name="roundBy">TimeSpan to round to.</param>
	/// <inheritdoc cref="Round(DateTime, TimeSpan)"/>
	public static DateTimeOffset Round(this DateTimeOffset dt, TimeSpan roundBy)
	{
		bool roundUp = _roundUp(dt.Ticks, roundBy);
		return roundUp ? dt.RoundUp(roundBy) : dt.RoundDown(roundBy);
	}

	static bool _roundUp(long ticks, TimeSpan roundBy)
	{
		long delta = ticks % roundBy.Ticks;
		return (delta * 2) >= roundBy.Ticks;
	}

	// --- RoundUp ---

	/// <summary>Rounds the DateTime up to the next multiple of <paramref name="d"/>; preserves <see cref="DateTime.Kind"/>.</summary>
	public static DateTime RoundUp(this DateTime dt, TimeSpan d)
	{
		long delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
		return new DateTime(dt.Ticks + delta, dt.Kind);
	}

	/// <inheritdoc cref="RoundUp(DateTime, TimeSpan)"/>
	public static DateTimeOffset RoundUp(this DateTimeOffset dt, TimeSpan d)
	{
		long delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
		return new DateTimeOffset(dt.Ticks + delta, dt.Offset);
	}

	// --- RoundDown ---

	/// <summary>Rounds the DateTime down to the previous multiple of <paramref name="d"/>; preserves <see cref="DateTime.Kind"/>.</summary>
	public static DateTime RoundDown(this DateTime dt, TimeSpan d)
	{
		long delta = dt.Ticks % d.Ticks;
		return new DateTime(dt.Ticks - delta, dt.Kind);
	}

	/// <inheritdoc cref="RoundDown(DateTime, TimeSpan)"/>
	public static DateTimeOffset RoundDown(this DateTimeOffset dt, TimeSpan d)
	{
		long delta = dt.Ticks % d.Ticks;
		return new DateTimeOffset(dt.Ticks - delta, dt.Offset);
	}
}
