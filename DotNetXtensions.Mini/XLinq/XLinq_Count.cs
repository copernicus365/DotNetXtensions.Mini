namespace DotNetXtensions;

public static partial class XLinq
{
	[DebuggerStepThrough]
	public static int LengthN<T>(this T[] arr, int defaultVal = 0)
		=> arr == null ? defaultVal : arr.Length;

	[DebuggerStepThrough]
	public static int LengthN(this string s, int defaultVal = 0)
		=> s == null ? defaultVal : s.Length;

	[DebuggerStepThrough]
	public static int CountN(this string s, int defaultVal = 0)
		=> s == null ? defaultVal : s.Length;

	[DebuggerStepThrough]
	public static int CountN<T>(this IList<T> list, int defaultVal = 0)
		=> list == null ? defaultVal : list.Count;

	[DebuggerStepThrough]
	public static int CountN<T>(this ICollection<T> coll, int defaultVal = 0)
		=> coll == null ? defaultVal : coll.Count;
}
