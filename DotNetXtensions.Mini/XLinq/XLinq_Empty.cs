namespace DotNetXtensions;

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

	/// <summary>Returns null if the value equals default, else returns the original value.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? NullIfDefault<T>(this T t) where T : struct
		=> EqualityComparer<T>.Default.Equals(t, default) ? null : t;

	/// <summary>Returns the specified value if the value equals default, else returns the original value.</summary>
	[DebuggerStepThrough]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ValueIfDefault<T>(this T t, T value) where T : struct
		=> EqualityComparer<T>.Default.Equals(t, default) ? value : t;


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
	//
	// Note: canNOT do IReadOnlyCollection<T> or IReadOnlyList<T> overloads, because IList<T> and ICollection<T> CONFLICT,
	// making eg List<T> NOT work! All BCL concrete types that implement IReadOnlyCollection<T> and IReadOnlyList<T>, like
	// ObservableCollection<T>, or ArraySegment<T>, also implement IList<T> and ICollection<T>.
}
