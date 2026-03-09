namespace DotNetXtensions;

public static partial class XDateTimes
{
	/// <summary>
	/// The .NET tick value at Unix Epoch (1970-01-01 00:00:00 UTC).
	/// Equivalent to <see cref="DateTime.UnixEpoch"/>.Ticks, but as a compile-time constant.
	/// </summary>
	public const long TicksAtUnixEpoch = 621355968000000000L;


	/// <summary>
	/// Converts Unix time milliseconds to .NET ticks.
	/// Similar to <see cref="DateTimeOffset.FromUnixTimeMilliseconds"/> but returns ticks directly without object allocation.
	/// </summary>
	/// <param name="value">Unix time in milliseconds since Epoch (1970-01-01 00:00:00 UTC).</param>
	public static long UnixTimeMillisecondsToTicks(this long value)
		=> TicksAtUnixEpoch + (value * 10000);

	/// <summary>
	/// Converts Unix time seconds to .NET ticks.
	/// Similar to <see cref="DateTimeOffset.FromUnixTimeSeconds"/> but returns ticks directly without object allocation.
	/// </summary>
	/// <param name="value">Unix time in seconds since Epoch (1970-01-01 00:00:00 UTC).</param>
	public static long UnixTimeSecondsToTicks(this long value)
		=> TicksAtUnixEpoch + (value * 10000000);


	/// <summary>
	/// Converts Unix time milliseconds to <see cref="DateTimeOffset"/>.
	/// Enables fluent conversion from <see cref="long"/> Unix timestamps received from APIs.
	/// </summary>
	/// <param name="value">Unix time in milliseconds since Epoch (1970-01-01 00:00:00 UTC).</param>
	/// <param name="offset">Time zone offset. If null, UTC (TimeSpan.Zero) is used.</param>
	public static DateTimeOffset UnixTimeMillisecondsToDateTimeOffset(this long value, TimeSpan? offset = null)
		=> new(value.UnixTimeMillisecondsToTicks(), offset ?? TimeSpan.Zero);

	/// <summary>
	/// Converts Unix time seconds to <see cref="DateTimeOffset"/>.
	/// Enables fluent conversion from <see cref="long"/> Unix timestamps received from APIs.
	/// </summary>
	/// <param name="value">Unix time in seconds since Epoch (1970-01-01 00:00:00 UTC).</param>
	/// <param name="offset">Time zone offset. If null, UTC (TimeSpan.Zero) is used.</param>
	public static DateTimeOffset UnixTimeSecondsToDateTimeOffset(this long value, TimeSpan? offset = null)
		=> new(value.UnixTimeSecondsToTicks(), offset ?? TimeSpan.Zero);
}
