namespace DotNetXtensions;

public static partial class XString
{
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
}
