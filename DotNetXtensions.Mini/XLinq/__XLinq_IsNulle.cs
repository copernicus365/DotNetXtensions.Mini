// namespace DotNetXtensions;

// public static partial class XLinq
// {
// 	[DebuggerStepThrough]
// 	public static bool IsNulle<TSource>(this TSource[] source)
// 		=> source == null || source.Length < 1;

// 	[DebuggerStepThrough]
// 	public static bool IsNulle<TSource>(this ICollection<TSource> source)
// 		=> source == null || source.Count < 1;

// 	[DebuggerStepThrough]
// 	public static bool NotNulle<TSource>(this ICollection<TSource> source)
// 		=> source != null && source.Count > 0;

// 	public static bool IsNulle<TValue>(this TValue? value) where TValue : struct
// 		=> value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default);

// 	public static bool NotNulle<TValue>(this TValue? value) where TValue : struct
// 		=> !IsNulle(value);
// }
