namespace DotNetXtensions;

public static partial class XLinq
{
	[DebuggerStepThrough]
	public static string JoinToString<T>(this IEnumerable<T> source, string separator = ",")
		=> source == null ? null : string.Join(separator, source);

	[DebuggerStepThrough]
	public static string JoinToString<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ",")
		=> source == null ? null : string.Join(separator, source.Select(selector));
}
