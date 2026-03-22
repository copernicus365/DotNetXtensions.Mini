using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetXtensions;


// ========== from file: PathX.cs ==========

/// <summary>Cleans up paths to use forward slashes, handles nulls gracefully, usually trims.</summary>
public static class PathX
{
	/// <summary>Calls `Path.GetFullPath`, but handles nulls, and cleans the path with `CleanPath`.</summary>
	public static string GetFullPath(string path)
		=> CleanPath(Path.GetFullPath(path ?? ""));

	public static string GetDirectoryName(string path)
		=> CleanPath(Path.GetDirectoryName(path ?? ""));

	public static string CleanPath(string path)
		=> (path ?? "")?.TrimIfNeeded()?.Replace('\\', '/');

	public static string PathCombine(string path1, string path2, bool trim = false)
		=> CleanPath(trim ? Path.Combine(path1.TrimIfNeeded(), path2.TrimIfNeeded()) : Path.Combine(path1, path2));
}


// ========== from file: XChar.cs ==========

public static class XChar
{
	/// <summary>
	/// Indicates whether the char is an ascii digit (0-9 only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiDigit(this char c)
		=> c < 58 && c > 47;

	/// <summary>
	/// Indicates whether the char is a lowercase ascii letter (a-z only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiLetter(this char c)
		=> (c > 96 && c < 123) || (c > 64 && c < 91);

	/// <summary>
	/// Indicates whether the char is a lowercase ascii letter (a-z only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiLower(this char c)
		=> c > 96 && c < 123;

	/// <summary>
	/// Indicates whether the char is an uppercase ascii letter (A-Z only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiUpper(this char c)
		=> c > 64 && c < 91;

	/// <summary>
	/// Indicates whether the char is a lowercase ascii letter or ascii digit (a-z || 0-9 only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiLowerOrDigit(this char c)
		=> (c > 96 && c < 123) || (c < 58 && c > 47);

	/// <summary>
	/// Indicates whether the char is an uppercase ascii letter or ascii digit (A-Z || 0-9 only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiUpperOrDigit(this char c)
		=> (c > 64 && c < 91) || (c < 58 && c > 47);

	/// <summary>
	/// Indicates whether the char is an ascii letter or ascii digit (a-z || A-Z || 0-9 only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiLetterOrDigit(this char c)
		=> (c > 96 && c < 123) || (c > 64 && c < 91) || (c < 58 && c > 47);


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsWhitespace(this char c)
		=> char.IsWhiteSpace(c);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsUpper(this char c)
		=> char.IsUpper(c);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLower(this char c)
		=> char.IsLower(c);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNumber(this char c)
		=> char.IsNumber(c);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ToInt(this char c)
		=> c - '0';
}


// ========== combined 3 partial files for: XDateTimes ==========


// ---
// --- partial: XDateTimes_EpochOSConversions.cs (0) ---
// ---


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


// ---
// --- partial: XDateTimes_OffsetConversions.cs (1) ---
// ---


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

	/// <summary>
	/// Converts to a new offset while representing the same moment in time (same UTC).
	/// The existing offset is discarded, and the local DateTime adjusts to the new offset.
	/// In a sense, is the opposite of <c>dt.ToOffset()</c>.
	/// </summary>
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

	/// <summary>
	/// Converts DateTime to DateTimeOffset, treating input as local time at the given timezone.
	/// The DateTime ticks are preserved. DateTime.Kind is ignored.
	/// </summary>
	public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeZoneInfo tzi)
		=> __ToDateTimeOffset(dt, tzi.GetUtcOffset(dt.ToUnspecifiedKindIfUtc()), isUtc: false);

	/// <summary>
	/// Converts DateTime to DateTimeOffset, treating input as UTC time. The local DateTime
	/// in the result is adjusted by the offset. DateTime.Kind is ignored.
	/// </summary>
	public static DateTimeOffset ToDateTimeOffsetFromUtc(this DateTime dt, TimeSpan offset)
		=> __ToDateTimeOffset(dt, offset, isUtc: true);

	/// <summary>
	/// Converts DateTime to DateTimeOffset, treating input as UTC time. The local DateTime
	/// in the result is adjusted by the timezone offset. DateTime.Kind is ignored.
	/// </summary>
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


// ---
// --- partial: XDateTimes_Round.cs (2) ---
// ---


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

	public static DateTime RoundUp(this DateTime dt, TimeSpan d)
	{
		long delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
		return new DateTime(dt.Ticks + delta, dt.Kind);
	}

	public static DateTimeOffset RoundUp(this DateTimeOffset dt, TimeSpan d)
	{
		long delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
		return new DateTimeOffset(dt.Ticks + delta, dt.Offset);
	}

	// --- RoundDown ---

	public static DateTime RoundDown(this DateTime dt, TimeSpan d)
	{
		long delta = dt.Ticks % d.Ticks;
		return new DateTime(dt.Ticks - delta, dt.Kind);
	}

	public static DateTimeOffset RoundDown(this DateTimeOffset dt, TimeSpan d)
	{
		long delta = dt.Ticks % d.Ticks;
		return new DateTimeOffset(dt.Ticks - delta, dt.Offset);
	}

}


// ========== from file: XDictionary_DictionariesAreEqual.cs ==========

public static partial class XDictionary
{
	public static bool DictionariesAreEqual<TKey, TValue>(
		this IDictionary<TKey, TValue> dict1,
		IDictionary<TKey, TValue> dict2,
		Func<TValue, TValue, bool> comparer = null)
	{
		if(dict1 == null || dict2 == null)
			return dict1 == null && dict2 == null;

		if(dict1.Count != dict2.Count)
			return false;

		bool hasEqCmpr = comparer != null;

		foreach(var kv in dict1) {
			TKey key1 = kv.Key;
			TValue val1 = kv.Value;

			if(!dict2.TryGetValue(key1, out TValue val2))
				return false;

			if(hasEqCmpr) {
				if(!comparer(val1, val2))
					return false;
			}
			else {
				if(!val1.Equals(val2))
					return false;
			}
		}
		return true;
	}
}


// ========== combined 3 partial files for: XLinq ==========


// ---
// --- partial: XLinq_Empty.cs (0) ---
// ---


public static partial class XLinq
{
	extension(string s)
	{
		/// <summary>Returns an empty string if null, else returns the string.</summary>
		public string EmptyIfNull => s ?? "";

		/// <summary>Returns null if empty string, else returns the string.</summary>
		public string NullIfEmpty => s == "" ? null : s;

		/// <summary>Returns null if empty string, else returns the string.</summary>
		public string NullIfWhitespace => string.IsNullOrWhiteSpace(s) ? null : s;

		/// <summary>Returns the length, or 0 if null.</summary>
		public int LengthN => s == null ? 0 : s.Length;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsEmpty => s == null || s.Length == 0;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotEmpty => s != null && s.Length != 0;

		/// <summary>Returns true if null or whitespace.</summary>
		public bool IsEmptyOrWhiteSpace => string.IsNullOrWhiteSpace(s);
	}

	extension<T>(T[] arr)
	{
		/// <summary>Returns an empty array if null, else returns the array.</summary>
		public T[] EmptyIfNull => arr ?? [];

		/// <summary>Returns the length, or 0 if null.</summary>
		public int LengthN => arr == null ? 0 : arr.Length;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsEmpty => arr == null || arr.Length < 1;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsNullOrEmpty => arr == null || arr.Length < 1;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotEmpty => arr != null && arr.Length > 0;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotNullOrEmpty => arr != null && arr.Length > 0;
	}

	extension<T>(IEnumerable<T> enumerable)
	{
		/// <summary>Returns an empty collection if null, else returns the collection.</summary>
		public IEnumerable<T> EmptyIfNull => enumerable ?? [];
	}

	// NullIfDefault

	extension<T>(T t) where T : struct
	{
		/// <summary>Returns null if the value equals default, else returns the original value.</summary>
		public T? NullIfDefault => EqualityComparer<T>.Default.Equals(t, default) ? null : t;

		/// <summary>Returns the specified value if the value equals default, else returns the original value.</summary>
		public T ValueIfDefault(T value) => EqualityComparer<T>.Default.Equals(t, default) ? value : t;
	}

	// CountN

	extension<T>(IList<T> list)
	{
		/// <summary>Returns the count, or 0 if null.</summary>
		public int CountN => list == null ? 0 : list.Count;
	}

	extension<T>(ICollection<T> coll)
	{
		/// <summary>Returns the count, or 0 if null.</summary>
		public int CountN => coll == null ? 0 : coll.Count;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsEmpty => coll == null || coll.Count < 1;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsNullOrEmpty => coll == null || coll.Count < 1;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotEmpty => coll != null && coll.Count > 0;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotNullOrEmpty => coll != null && coll.Count > 0;
	}

	extension<TValue>(TValue? value) where TValue : struct
	{
		/// <summary>Returns true if null or equals the default value.</summary>
		public bool IsDefault => value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns true if null or equals the default value.</summary>
		public bool IsNullOrDefault => value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns true if not null and not equal to the default value.</summary>
		public bool NotDefault => value != null && !EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns true if not null and not equal to the default value.</summary>
		public bool NotNullOrDefault => value != null && !EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns the value if not null, else default if null.</summary>
		public TValue ValueOrDefault => value ?? default;

		/// <summary>Returns the value if it is set (not null and not default), else returns input 'or' value.</summary>
		public TValue ValueOr(TValue alt) => value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default) ? alt : value.Value;
	}

	extension<T>(T t) where T : class, new()
	{
		/// <summary>Returns a new instance if null, else returns the object.</summary>
		public T EmptyIfNull => t ?? new T();
	}


// ---
// --- partial: XLinq_IfLinq.cs (1) ---
// ---


// --- WhereIf ---

	/// <summary>Applies Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <summary>Applies indexed Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <summary>Applies Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <summary>Applies indexed Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicate)
		=> !condition ? source : source?.Where(predicate);


	// --- WhereIfElse ---

	/// <summary>Applies one of two Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicateIf, Func<T, bool> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <summary>Applies one of two indexed Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicateIf, Func<T, int, bool> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <summary>Applies one of two Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicateIf, Expression<Func<T, bool>> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <summary>Applies one of two indexed Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicateIf, Expression<Func<T, int, bool>> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);


	// --- SkipIf ---

	/// <summary>Skips elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> SkipIf<T>(this IEnumerable<T> source, bool condition, int count)
		=> !condition ? source : source?.Skip(count);

	/// <summary>Skips elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> SkipIf<T>(this IQueryable<T> source, bool condition, int count)
		=> !condition ? source : source?.Skip(count);


	// --- TakeIf ---

	/// <summary>Takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> source, bool condition, int count)
		=> !condition ? source : source?.Take(count);

	/// <summary>Takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> TakeIf<T>(this IQueryable<T> source, bool condition, int count)
		=> !condition ? source : source?.Take(count);


	// --- SkipTakeIf ---

	/// <summary>Skips then takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> SkipTakeIf<T>(this IEnumerable<T> source, bool condition, int skip, int count)
		=> !condition ? source : source?.Skip(skip).Take(count);

	/// <summary>Skips then takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> SkipTakeIf<T>(this IQueryable<T> source, bool condition, int skip, int count)
		=> !condition ? source : source?.Skip(skip).Take(count);


// ---
// --- partial: XLinq_JoinToString.cs (2) ---
// ---


[DebuggerStepThrough]
	public static string JoinToString<T>(this IEnumerable<T> source, string separator = ",")
		=> source == null ? null : string.Join(separator, source);

	[DebuggerStepThrough]
	public static string JoinToString<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ",")
		=> source == null ? null : string.Join(separator, source.Select(selector));

}


// ========== from file: XNumber_InRange.cs ==========

public static partial class XNumber
{
	// --- InRange / NotInRange ---

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool InRange<T>(this T val, T val1, T val2)
		where T : INumber<T>, IComparisonOperators<T, T, bool>
		=> val >= val1 && val <= val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotInRange<T>(this T val, T val1, T val2)
		where T : INumber<T>, IComparisonOperators<T, T, bool>
		=> val < val1 || val > val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool LengthInRange(this string val, int val1, int val2)
		=> val != null && val.Length.InRange(val1, val2);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool LengthNotInRange(this string val, int val1, int val2)
		=> val != null && val.Length.NotInRange(val1, val2);


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool InRange(this TimeSpan val, TimeSpan val1, TimeSpan val2)
		=> val >= val1 && val <= val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotInRange(this TimeSpan val, TimeSpan val1, TimeSpan val2)
		=> val < val1 || val > val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool InRange(this DateTime val, DateTime val1, DateTime val2)
		=> val >= val1 && val <= val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotInRange(this DateTime val, DateTime val1, DateTime val2)
		=> val < val1 || val > val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool InRange(this DateTimeOffset val, DateTimeOffset val1, DateTimeOffset val2)
		=> val >= val1 && val <= val2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotInRange(this DateTimeOffset val, DateTimeOffset val1, DateTimeOffset val2)
		=> val < val1 || val > val2;

}


// ========== from file: XNewLines.cs ==========

/// <summary>
/// Extension methods for splitting strings based on new lines. Provides efficient line enumeration
/// including lazy enumeration with true deferred execution.
/// </summary>
public static class XNewLines
{
	/// <summary>
	/// Replaces Windows-style carriage return line endings ("\r\n") with Unix-style
	/// line feed characters ('\n'). Optionally checks first if this operation is needed.
	/// Handles null or empty input safely.
	/// </summary>
	/// <param name="s">String input. Can be null or empty.</param>
	/// <param name="ifNeeded">If true, checks first if the operation is needed by testing
	/// if any '\r' characters exist. If none found, returns the original string unchanged.</param>
	/// <returns>The string with all "\r\n" sequences replaced with "\n". Returns the original
	/// string if null, empty, or if <paramref name="ifNeeded"/> is true and no '\r' found.</returns>
	public static string ToUnixLines(this string s, bool ifNeeded = false)
	{
		if(s.IsEmpty)
			return s;

		if(ifNeeded && s.IndexOf('\r') < 0)
			return s;

		return s.Replace("\r\n", "\n");
	}

	/// <summary>
	/// Splits a string into an array of lines, based on string.Split with appropriate line ending delimiters.
	/// This is *entirely* based on string.Split, a simple wrapper, but adds some convenience and consistency.
	/// </summary>
	/// <param name="s">String input. Can be null or empty.</param>
	/// <param name="trim">If true, trims leading and trailing whitespace from each line.</param>
	/// <param name="ignoreEmpty">If true, removes empty lines from the result.
	/// Applied after trimming if <paramref name="trim"/> is also true.</param>
	/// <param name="unixOnly">If true, only splits on LF (\n) characters, ignoring CR (\r).
	/// Useful when you know the input only has Unix-style line endings for better performance.</param>
	/// <returns>Array of lines. Returns empty if null or empty input.</returns>
	public static string[] GetLines(this string s, bool trim = false, bool ignoreEmpty = false, bool unixOnly = false)
	{
		if(s.IsEmpty)
			return [];

		StringSplitOptions options = StringSplitOptions.None;
		if(ignoreEmpty) options |= StringSplitOptions.RemoveEmptyEntries;
		if(trim) options |= StringSplitOptions.TrimEntries;

		return unixOnly
			? s.Split('\n', options)
			: s.Split(["\r\n", "\n", "\r"], options);
	}

	/// <summary>
	/// Enumerates lines lazily with true deferred execution, ideal for LINQ operations,
	/// using efficient span based processing. Only processes and allocates strings for lines that
	/// are actually consumed. Supports LINQ operations like Take(), First(), Where(), etc,
	/// without processing the entire string.
	/// </summary>
	/// <param name="s">String input. Can be null or empty.</param>
	/// <param name="trim">If true, trims leading and trailing whitespace from each line.
	/// Trimming is performed on ReadOnlySpan level before string allocation for efficiency.</param>
	/// <param name="ignoreEmpty">If true, skips empty lines (or lines that become empty after trimming).</param>
	/// <param name="unixOnly">If true, only splits on LF (\n) characters, ignoring CR (\r).
	/// Use when input is known to have Unix-style line endings only.</param>
	/// <returns>Lazy enumerable of lines. Call .ToArray() if you need all lines materialized upfront.</returns>
	public static IEnumerable<string> GetLinesLazy(this string s, bool trim = false, bool ignoreEmpty = false, bool unixOnly = false)
	{
		if(s.IsEmpty)
			yield break;

		int pos = 0;
		int len = s.Length;

		while(pos <= len) {
			int newlineIndex = -1;

			if(pos < len) {
				var span = s.AsSpan(pos);
				newlineIndex = unixOnly
					? span.IndexOf('\n')
					: span.IndexOfAny('\r', '\n');
			}

			int lineStart = pos;
			int lineEnd;

			if(newlineIndex == -1) {
				// Last line (or empty line after trailing newline)
				lineEnd = len;
				pos = len + 1; // Move past end to terminate loop
			}
			else {
				lineEnd = pos + newlineIndex;

				// Skip the newline character(s) - handle both \r\n and single \n or \r
				pos = lineEnd + 1;
				if(!unixOnly && pos < len && s[lineEnd] == '\r' && s[pos] == '\n') {
					pos++; // Skip \n after \r
				}
			}

			ReadOnlySpan<char> line = s.AsSpan(lineStart, lineEnd - lineStart);

			if(trim)
				line = line.Trim();

			if(ignoreEmpty && line.IsEmpty)
				continue;

			yield return line.ToString();
		}
	}

#if NET9_0_OR_GREATER

	/// <summary>
	/// Iterates through lines using a callback function, with support for early termination.
	/// Uses the highly optimized ReadOnlySpan&lt;char&gt;.EnumerateLines() internally for maximum performance.
	/// Handles all line ending types automatically (CRLF, LF, CR).
	/// Available in .NET 9+ only.
	/// </summary>
	/// <param name="s">String input. Can be null or empty.</param>
	/// <param name="act">Callback invoked for each line. Return false to stop enumeration immediately,
	/// return true to continue to the next line.</param>
	/// <param name="trim">If true, trims leading and trailing whitespace from each line before passing
	/// to the callback. Trimming is performed on ReadOnlySpan level for efficiency.</param>
	/// <param name="ignoreEmpty">If true, skips empty lines (or lines that become empty after trimming).
	/// The callback will not be invoked for skipped lines.</param>
	/// <remarks>
	/// This method uses the .NET runtime's SpanLineEnumerator internally, which provides
	/// vectorized/SIMD-optimized line scanning. This is the fastest line enumeration method
	/// when you need to process lines sequentially without LINQ operations.
	///
	/// Prefer this over EnumerateLinesLazy when:
	/// - You need maximum performance for sequential processing
	/// - You want to stop early based on runtime conditions
	/// - You don't need LINQ composition
	/// </remarks>
	/// <example>
	/// <code>
	/// // Process until specific line found
	/// string result = null;
	/// text.ForEachLine(line => {
	///     if (line.StartsWith("Result:")) {
	///         result = line;
	///         return false; // Stop processing
	///     }
	///     return true; // Continue
	/// });
	///
	/// // Count non-empty lines
	/// int count = 0;
	/// text.ForEachLine(line => { count++; return true; }, ignoreEmpty: true);
	/// </code>
	/// </example>
	public static void ForEachLine(this string s, Func<string, bool> act, bool trim = false, bool ignoreEmpty = false)
	{
		if(s.IsEmpty)
			return;

		foreach(var line in s.AsSpan().EnumerateLines()) {
			ReadOnlySpan<char> ln = trim ? line.Trim() : line;

			if(ignoreEmpty && ln.IsEmpty)
				continue;

			if(!act(ln.ToString()))
				break;
		}
	}

#endif
}


// ========== combined 5 partial files for: XString ==========


// ---
// --- partial: XString_FirstNotNulle.cs (0) ---
// ---


public static partial class XString
{
	/// <summary>
	/// Returns first input string that is not null or empty. If all are null or empty, returns null.
	/// </summary>
	/// <param name="value1">Value 1.</param>
	/// <param name="value2">Value 2.</param>
	/// <param name="value3">Value 3.</param>
	[DebuggerStepThrough]
	public static string FirstNotNulle(this string value1, string value2, string value3 = null)
	{
		if(value1 != null && value1.Length > 0)
			return value1;

		if(value2 != null && value2.Length > 0)
			return value2;

		if(value3 != null && value3.Length > 0)
			return value3;

		return null;
	}


// ---
// --- partial: XString_Print.cs (1) ---
// ---


// maybe a mistake adding this ... hmmm

	[DebuggerStepThrough]
	public static string Print(this string s)
	{
		Console.WriteLine(s);
		return s;
	}

	[DebuggerStepThrough]
	public static void Print(this object obj)
		=> Console.Write(obj);


// ---
// --- partial: XString_SubstringMax.cs (2) ---
// ---


public static string SubstringMax(this string str, int maxLength, string ellipsis = null, bool tryBreakOnWord = false)
		=> SubstringMax(str, 0, maxLength, ellipsis: ellipsis, tryBreakOnWord: tryBreakOnWord);

	/// <summary>
	/// Returns a substring of the input string where instead of specifying
	/// the exact length of the return string (which in .NET's string.Substring
	/// cannot be specified out of range), one specifies a maxLength, meaning
	/// maxLength can be out of range, in which case the substring from index
	/// to the end of the string is returned.
	/// <para />
	/// If the string is NULL or EMPTY, the same is immediately returned, NO exceptions
	/// (Null or OutOfRange) will be thrown.
	/// <para/>
	/// This nicely solves the problem when one simply wants the first n length
	/// of characters from a string, but without having to write a bunch of
	/// code to make sure they do not go out of range in case, for instance, the string was shorter
	/// than expected.
	/// </summary>
	/// <param name="str">String</param>
	/// <param name="index">Start Index</param>
	/// <param name="maxLength">Maximum length of the return substring.</param>
	/// <param name="ellipsis">...</param>
	/// <param name="tryBreakOnWord"></param>
	public static string SubstringMax(this string str, int index, int maxLength, string ellipsis = null, bool tryBreakOnWord = false)
	{
		const int maxWordBreakSize = 15;
		const int minCheckWordBreak = 7;

		if(str == null || str.Length == 0) return str;

		int strLen = str.Length;
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, strLen);

		if(index == 0 && strLen <= maxLength)
			return str;

		int finalLen = strLen - index;
		bool useEllipsis = ellipsis.NotEmpty;

		if(maxLength < finalLen)
			finalLen = maxLength;
		else
			useEllipsis = false; // was true up to here if wasn't nulle

		// WOULD BE MORE PERFORMANT TO DO THE WORD-BREAK SEARCH HERE,
		// NOT NEED AN EXTRA STRING ALLOC. WANNA DO IT?! GO AHEAD, MAKE MY DAY!

		int postIdx = index + finalLen;
		string result = str.Substring(index, finalLen);

		// WORD-BREAK SEARCH
		if(tryBreakOnWord && postIdx < strLen && char.IsLetterOrDigit(str, postIdx) && result.Length > minCheckWordBreak) {
			int i = 0;
			int x = result.Length - 1;
			for(; i < maxWordBreakSize && x >= minCheckWordBreak; i++, x--) {
				if(char.IsWhiteSpace(result[x]))
					break;
			}
			if(i > 0 && i < maxWordBreakSize && x >= minCheckWordBreak)
				result = result.Substring(0, x + 1);
		}

		if(useEllipsis)
			result += ellipsis;

		return result;
	}


// ---
// --- partial: XString_ToValue.cs (3) ---
// ---


[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ToInt(this string val, int dflt = 0)
		=> int.TryParse(val, out int v) ? v : dflt;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long ToLong(this string val, long dflt = 0)
		=> long.TryParse(val, out long v) ? v : dflt;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal ToDecimal(this string val, decimal dflt = 0)
		=> decimal.TryParse(val, out decimal v) ? v : dflt;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double ToDouble(this string val, double dflt = 0)
		=> double.TryParse(val, out double v) ? v : dflt;

	public static bool ToBool(this string val, bool dflt = false)
	{
		if(val.NotEmpty) {
			if(bool.TryParse(val, out bool i))
				return i;

			if(int.TryParse(val, out int num) && num < 2) {
				if(num == 0) return false;
				if(num == 1) return true;
			}
		}
		return dflt;
	}

	public static DateTime ToDateTime(this string val, DateTime? defaultVal = null)
	{
		if(val.NotEmpty) {
			// NOTE!!  parses DateTimeOffset and if true gets the DateTime...
			// big issues with frameword using computer's local time in these considerations,
			// this was probably not wise though :think: ... but change now would be breaking...
			if(DateTimeOffset.TryParse(val, out DateTimeOffset dt))
				return dt.DateTime;
		}
		return defaultVal ?? DateTime.MinValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTimeOffset ToDateTimeOffset(this string val, DateTimeOffset? dflt = null)
		=> DateTimeOffset.TryParse(val, out DateTimeOffset v) ? v : (dflt ?? DateTimeOffset.MinValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid ToGuid(this string val, Guid? dflt = null)
		=> Guid.TryParse(val, out Guid v) ? v : (dflt ?? default);



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int? ToIntN(this string val)
		=> val.NotEmpty && int.TryParse(val, out int v) ? v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long? ToLongN(this string val)
		=> val.NotEmpty && long.TryParse(val, out long v) ? v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal? ToDecimalN(this string val)
		=> val.NotEmpty && decimal.TryParse(val, out decimal v) ? v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double? ToDoubleN(this string val)
		=> val.NotEmpty && double.TryParse(val, out double v) ? v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool? ToBoolN(this string val)
		=> val.IsEmpty ? null : ToBool(val); // must use ToBool, handles numeric...

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime? ToDateTimeN(this string val)
		=> val.NotEmpty && DateTimeOffset.TryParse(val, out DateTimeOffset v) // see notes above: ToDateTime()
		? v.DateTime : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTimeOffset? ToDateTimeOffsetN(this string val)
		=> val.NotEmpty && DateTimeOffset.TryParse(val, out DateTimeOffset v) ? v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid? ToGuidN(this string val)
		=> val.NotEmpty && Guid.TryParse(val, out Guid v) ? v : null;


// ---
// --- partial: XString_Trim.cs (4) ---
// ---


extension(string s)
	{
		public bool IsTrimmable
			=> s != null && s.Length > 0 && (char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]));

		/// <summary>Trims the string only if it is needed. Value CAN be Null or Empty.</summary>
		[DebuggerStepThrough]
		public string TrimIfNeeded()
		{
			if(s == null || s.Length == 0) return s;
			if(char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]))
				return s.Trim();
			return s;
		}

		[DebuggerStepThrough]
		public string TrimToNull()
		{
			if(s == null || s.Length == 0) return null;
			if(char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]))
				s = s.Trim();
			return s.Length == 0 ? null : s;
		}

		/// <summary>Trims the string if it is not null, else returns null.</summary>
		[DebuggerStepThrough]
		public string TrimN() => s?.Trim();
	}

}

