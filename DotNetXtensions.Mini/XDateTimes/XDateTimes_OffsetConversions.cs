namespace DotNetXtensions;

/// <summary>
/// Extension methods for <see cref="DateTime"/> and <see cref="DateTimeOffset"/> — Unix epoch conversions,
/// offset/timezone conversions, and rounding.
/// </summary>
public static partial class XDateTimes
{
	/// <summary>
	/// Convenience overload for converting to a new offset based on TimeZoneInfo.
	/// Equivalent to <c>dt.ToOffset(tzi.GetUtcOffset(dt))</c>.
	/// </summary>
	public static DateTimeOffset ToOffset(this DateTimeOffset dt, TimeZoneInfo tzi)
		=> dt.ToOffset(tzi.GetUtcOffset(dt));

	/// <summary>
	/// Converts to a new offset while representing the same moment in time (same UTC).
	/// The existing offset is discarded, and the local DateTime adjusts to the new offset.
	/// In a sense, is the opposite of <c>dt.ToOffset()</c>.
	/// </summary>
	public static DateTimeOffset ToOffsetSameUtc(this DateTimeOffset dt, TimeZoneInfo tzi)
		=> __convertOffsetDTO_UTCFixed(dt, tzi.GetUtcOffset(dt));

	/// <inheritdoc cref="ToOffsetSameUtc(DateTimeOffset, TimeZoneInfo)"/>
	public static DateTimeOffset ToOffsetSameUtc(this DateTimeOffset dt, TimeSpan offset)
		=> __convertOffsetDTO_UTCFixed(dt, offset);



	static DateTimeOffset __convertOffsetDTO_UTCFixed(DateTimeOffset dt, TimeSpan offset)
	{
		if(dt == DateTimeOffset.MinValue)
			return DateTimeOffset.MinValue;

		AssertDateTZOffsetInRange(offset);

		long ticks = dt.UtcDateTime.Ticks + offset.Ticks;
		if(ticks <= 0)
			return DateTimeOffset.MinValue;

		if(ticks > _maxDTTicks)
			return DateTimeOffset.MaxValue;

		DateTimeOffset val = new(ticks, offset);
		return val;
	}


	/// <summary>
	/// Converts DateTime to DateTimeOffset, treating input as local time at the input offset.
	/// The DateTime ticks are preserved. DateTime.Kind is ignored.
	/// </summary>
	public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeSpan offset)
		=> __ToDateTimeOffset(dt, offset, isUtc: false);

	/// <inheritdoc cref="ToDateTimeOffset(DateTime, TimeSpan)"/>
	public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeZoneInfo tzi)
		=> __ToDateTimeOffset(dt, tzi.GetUtcOffset(dt.ToUnspecifiedKindIfUtc()), isUtc: false);

	/// <summary>
	/// Converts DateTime to DateTimeOffset, treating input as UTC time. The local DateTime
	/// in the result is adjusted by the offset. DateTime.Kind is ignored.
	/// </summary>
	public static DateTimeOffset ToDateTimeOffsetFromUtc(this DateTime dt, TimeSpan offset)
		=> __ToDateTimeOffset(dt, offset, isUtc: true);

	/// <inheritdoc cref="ToDateTimeOffsetFromUtc(DateTime, TimeSpan)"/>
	public static DateTimeOffset ToDateTimeOffsetFromUtc(this DateTime dt, TimeZoneInfo tzi)
		=> __ToDateTimeOffset(dt, tzi.GetUtcOffset(dt.ToUnspecifiedKindIfUtc()), isUtc: true);


	/// <summary>
	/// Converts input DateTime to a new DateTimeOffset with the specified Offset value. 
	/// Importantly, the Kind property on the input DateTime *is ignored* 
	/// (the framework unfortunately throws an exception if <c>dt.Kind == DateTimeKind.Utc</c>).
	/// Set the <paramref name="isUtc"/> value to true (default is true) to indicate input DateTime is
	/// already UTC time, or false to be treated as a local time (any non-UTC time).
	/// </summary>
	/// <param name="dt">DateTime</param>
	/// <param name="offset">Offset</param>
	/// <param name="isUtc">Indicates if the input DateTime is already a UTC value 
	/// or else a Local value (any time that is not UTC).</param>
	static DateTimeOffset __ToDateTimeOffset(DateTime dt, TimeSpan offset, bool isUtc = true)
	{
		if(dt == DateTime.MinValue)
			return DateTimeOffset.MinValue;

		AssertDateTZOffsetInRange(offset);

		// convert with Ticks, bypass the framework's ugly exception on DateTime.Kind when not as expected
		long ticks = isUtc
			? dt.Ticks + offset.Ticks // dt is UTC, must convert to LOCAL time
			: dt.Ticks; // dt is ALREADY local time, no change

		if(ticks <= 0)
			return DateTimeOffset.MinValue;

		if(ticks > _maxDTTicks)
			return DateTimeOffset.MaxValue;

		DateTimeOffset val = new(ticks, offset);
		return val;
	}

	/// <summary>
	/// Converts DateTime.Kind to Unspecified if it was UTC, otherwise returns unchanged.
	/// Useful for bypassing framework exceptions when DateTime.Kind conflicts with intended usage.
	/// </summary>
	public static DateTime ToUnspecifiedKindIfUtc(this DateTime dt)
		=> dt.Kind != DateTimeKind.Utc ? dt : new DateTime(dt.Ticks, DateTimeKind.Unspecified);



	static void AssertDateTZOffsetInRange(TimeSpan offset)
	{
		if(offset.NotInRange(_utcOffsetMin, _utcOffsetMax))
			throw new ArgumentOutOfRangeException(nameof(offset));
	}

	static readonly TimeSpan _utcOffsetMin = TimeSpan.FromHours(-14);
	static readonly TimeSpan _utcOffsetMax = TimeSpan.FromHours(14);
	static readonly long _maxDTTicks = DateTimeOffset.MaxValue.Ticks;
}
