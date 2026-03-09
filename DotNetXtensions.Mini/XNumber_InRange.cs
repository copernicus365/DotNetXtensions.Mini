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
