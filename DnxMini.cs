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


// ========== from file: XDateTimes_Round.cs ==========

public static class XDateTimes
{
	// --- ROUND DATE/TIME ---

	/// <summary>
	/// Rounds the DateTime to the nearest specified interval.
	/// <para/>
	/// Thanks to DevSal on http://stackoverflow.com/questions/7029353/c-sharp-round-up-time-to-nearest-x-minutes.
	/// </summary>
	/// <param name="dt">DateTime to round.</param>
	/// <param name="roundBy">TimeSpan to round to.</param>
	public static DateTime Round(this DateTime dt, TimeSpan roundBy)
		=> new DateTime(_RoundTicks(roundBy, dt.Ticks));

	/// <summary>
	/// Rounds the DateTimeOffset to the nearest specified interval.
	/// <para/>
	/// Thanks to DevSal on http://stackoverflow.com/questions/7029353/c-sharp-round-up-time-to-nearest-x-minutes.
	/// </summary>
	/// <param name="dt">DateTime to round.</param>
	/// <param name="roundBy">TimeSpan to round to.</param>
	public static DateTimeOffset Round(this DateTimeOffset dt, TimeSpan roundBy)
		=> new DateTimeOffset(_RoundTicks(roundBy, dt.Ticks), dt.Offset);

	static long _RoundTicks(TimeSpan roundBy, long dtTicks)
	{
		long roundTicks = roundBy.Ticks;
		int f = 0;
		double m = (double)(dtTicks % roundTicks) / roundTicks;
		if(m >= 0.5)
			f = 1;
		long val = ((dtTicks / roundTicks) + f) * roundTicks;
		return val;
	}


	// http://stackoverflow.com/questions/7029353/how-can-i-round-up-the-time-to-the-nearest-x-minutes

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



	public static DateTime RoundToNearest(this DateTime dt, TimeSpan d)
	{
		long delta = dt.Ticks % d.Ticks;
		bool roundUp = delta > d.Ticks / 2;
		return roundUp ? dt.RoundUp(d) : dt.RoundDown(d);
	}

	public static DateTimeOffset RoundToNearest(this DateTimeOffset dt, TimeSpan d)
	{
		long delta = dt.Ticks % d.Ticks;
		bool roundUp = delta > d.Ticks / 2;
		return roundUp ? dt.RoundUp(d) : dt.RoundDown(d);
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


// ========== combined 4 partial files for: XLinq ==========


// ---
// --- partial: XLinq_Empty.cs (0) ---
// ---


public static partial class XLinq
{
	/// <summary>Returns a new instance if null, else returns the object.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T EmptyIfNull<T>(this T t) where T : class, new()
		=> t ?? new T();

	/// <summary>Returns an empty string if null, else returns the string.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string EmptyIfNull(this string s)
		=> s ?? "";

	/// <summary>Returns an empty array if null, else returns the array.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T[] EmptyIfNull<T>(this T[] array)
		=> array ?? [];

	/// <summary>Returns an empty collection if null, else returns the collection.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
		=> enumerable ?? [];

	// ---

	/// <summary>Gets the value or default if null. Alias for GetValueOrDefault</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ValueOrDefault<T>(this T? t) where T : struct
		=> t.GetValueOrDefault();

	/// <summary>Gets the value or the input default value if null. Alias for GetValueOrDefault</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ValueOrDefault<T>(this T? t, T defaultValue) where T : struct
		=> t.GetValueOrDefault(defaultValue);

	// ---

	/// <summary>Returns null if the value equals default, else returns the value.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? NullIfDefault<T>(this T t) where T : struct
		=> t.Equals(default(T)) ? null : t;


	// ---

	/// <summary>Returns the length, or default if null.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int LengthN<T>(this T[] arr, int defaultValue = 0)
		=> arr == null ? defaultValue : arr.Length;

	/// <summary>Returns the length, or default if null.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int LengthN(this string s, int defaultValue = 0)
		=> s == null ? defaultValue : s.Length;

	/// <summary>Returns the count, or default if null.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int CountN<T>(this IList<T> list, int defaultValue = 0)
		=> list == null ? defaultValue : list.Count;

	/// <summary>Returns the count, or default if null.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int CountN<T>(this ICollection<T> coll, int defaultValue = 0)
		=> coll == null ? defaultValue : coll.Count;

	/// <summary>Returns the count, or default if null.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int CountN<T>(this IReadOnlyCollection<T> coll, int defaultValue = 0)
		=> coll == null ? defaultValue : coll.Count;

	/// <summary>Returns the count, or default if null.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int CountN<T>(this IReadOnlyList<T> list, int defaultValue = 0)
		=> list == null ? defaultValue : list.Count;


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
// --- partial: XLinq_IsNulle.cs (2) ---
// ---


[DebuggerStepThrough]
	public static bool IsNulle<TSource>(this TSource[] source)
		=> source == null || source.Length < 1;

	[DebuggerStepThrough]
	public static bool IsNulle<TSource>(this ICollection<TSource> source)
		=> source == null || source.Count < 1;

	[DebuggerStepThrough]
	public static bool NotNulle<TSource>(this ICollection<TSource> source)
		=> source != null && source.Count > 0;

	public static bool IsNulle<TValue>(this Nullable<TValue> value) where TValue : struct
		=> value == null || value.Value.Equals(default(TValue));

	public static bool NotNulle<TValue>(this Nullable<TValue> value) where TValue : struct
		=> !IsNulle(value);


// ---
// --- partial: XLinq_JoinToString.cs (3) ---
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
	public static bool InRange(this string val, int val1, int val2)
		=> val != null && val.Length.InRange(val1, val2);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotInRange(this string val, int val1, int val2)
		=> val != null && val.Length.NotInRange(val1, val2);
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
		if(s.IsNulle())
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
		if(s.IsNulle())
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
		if(s.IsNulle())
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
		if(s.IsNulle())
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


// ========== combined 6 partial files for: XString ==========


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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
// --- partial: XString_Nulle.cs (1) ---
// ---


// --- string null/empty ---

	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNulle(this string str)
		=> str == null || str.Length == 0;

	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNulle(this string str)
		=> str != null && str.Length != 0;

	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty(this string str)
		=> str == null || str.Length == 0;

	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrWhiteSpace(this string str)
		=> string.IsNullOrWhiteSpace(str);

	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string NullIfEmpty(this string s)
		=> s == "" ? null : s;


// ---
// --- partial: XString_Print.cs (2) ---
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
// --- partial: XString_SubstringMax.cs (3) ---
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
		bool useEllipsis = ellipsis.NotNulle();

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
// --- partial: XString_ToValue.cs (4) ---
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ToBool(this string val, bool dflt = false)
	{
		if(val.NotNulle()) {
			if(bool.TryParse(val, out bool i))
				return i;

			if(int.TryParse(val, out int num) && num < 2) {
				if(num == 0) return false;
				if(num == 1) return true;
			}
		}
		return dflt;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime ToDateTime(this string val, DateTime? defaultVal = null)
	{
		if(val.NotNulle()) {
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
		=> val.NotNulle() && int.TryParse(val, out int v) ? (int?)v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long? ToLongN(this string val)
		=> val.NotNulle() && long.TryParse(val, out long v) ? (long?)v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal? ToDecimalN(this string val)
		=> val.NotNulle() && decimal.TryParse(val, out decimal v) ? (decimal?)v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double? ToDoubleN(this string val)
		=> val.NotNulle() && double.TryParse(val, out double v) ? (double?)v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool? ToBoolN(this string val)
		=> val.IsNulle() ? (bool?)null : ToBool(val); // must use ToBool, handles numeric...

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime? ToDateTimeN(this string val)
		=> val.NotNulle() && DateTimeOffset.TryParse(val, out DateTimeOffset v) // see notes above: ToDateTime()
		? (DateTime?)v.DateTime : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTimeOffset? ToDateTimeOffsetN(this string val)
		=> val.NotNulle() && DateTimeOffset.TryParse(val, out DateTimeOffset v) ? (DateTimeOffset?)v : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid? ToGuidN(this string val)
		=> val.NotNulle() && Guid.TryParse(val, out Guid v) ? (Guid?)v : null;


// ---
// --- partial: XString_Trim.cs (5) ---
// ---


[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string NullIfEmptyTrimmed(this string s)
	{
		s = s.TrimIfNeeded();
		return s == "" ? null : s;
	}

	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsTrimmable(this string s)
	{
		if(s == null || s.Length < 1)
			return false;
		return char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]);
	}

	/// <summary>
	/// Trims the string only if it is needed. Value CAN be Null or Empty.
	/// </summary>
	/// <param name="s">String</param>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TrimIfNeeded(ref string s)
	{
		if(s.IsTrimmable()) {
			s = s.Trim();
			return true;
		}
		return false;
	}

	/// <summary>
	/// Trims the string only if it is needed. Value CAN be Null or Empty.
	/// </summary>
	/// <param name="s">String</param>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string TrimIfNeeded(this string s)
	{
		if(s.IsTrimmable())
			return s.Trim();
		return s;
	}

	/// <summary>
	/// Trims the string if it is not null, else returns null.
	/// </summary>
	/// <param name="s">String</param>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string TrimN(this string s)
		=> s?.Trim();

}

