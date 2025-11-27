using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DotNetXtensions.Mini;

public static class DnxMini // single file!!
{
	#region --- Collection null/empty ---

	[DebuggerStepThrough]
	public static bool IsNulle<TSource>(this TSource[] source)
		=> source == null || source.Length < 1;

	[DebuggerStepThrough]
	public static bool IsNulle<TSource>(this ICollection<TSource> source)
		=> source == null || source.Count < 1;

	[DebuggerStepThrough]
	public static bool NotNulle<TSource>(this ICollection<TSource> source)
		=> source != null && source.Count > 0;

	#endregion

	#region --- string null/empty ---

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

	#endregion

	#region --- LengthN / CountN ---

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

	#endregion

	#region --- string trim ---

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
		if(s != null) {
			int len = s.Length;
			if(len > 1)
				return char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[len - 1]);
			return len == 0 || char.IsWhiteSpace(s[0]);
		}
		return false;
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
		=> s == null ? null : s.Trim();


	#endregion

	#region --- char ---

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

	#endregion
}
