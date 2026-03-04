using System.Numerics;

namespace DotNetXtensions;

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
