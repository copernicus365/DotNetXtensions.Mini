namespace DotNetXtensions;

public static partial class XLinq
{
	/// <summary>Joins the elements of <paramref name="source"/> into a string using <paramref name="separator"/>; returns null if source is null.</summary>
	[DebuggerStepThrough]
	public static string JoinToString<T>(this IEnumerable<T> source, string separator = ",")
		=> source == null ? null : string.Join(separator, source);

	/// <inheritdoc cref="JoinToString{T}(IEnumerable{T}, string)"/>
	[DebuggerStepThrough]
	public static string JoinToString<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ",")
		=> source == null ? null : string.Join(separator, source.Select(selector));
}
