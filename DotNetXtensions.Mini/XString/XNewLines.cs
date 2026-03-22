namespace DotNetXtensions;

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
