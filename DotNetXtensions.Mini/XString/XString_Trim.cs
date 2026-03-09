namespace DotNetXtensions;

public static partial class XString
{
	[DebuggerStepThrough]
	public static string NullIfEmptyTrimmed(this string s)
	{
		s = s.TrimIfNeeded();
		return s == "" ? null : s;
	}

	[DebuggerStepThrough]
	public static bool IsTrimmable(this string s)
	{
		if(s == null || s.Length < 1)
			return false;
		return char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]);
	}

	/// <summary>
	/// Trims the string only if it is needed. Value CAN be Null or Empty.
	/// </summary>
	/// <param name="s">String</param>
	[DebuggerStepThrough]
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
		=> s?.Trim();
}
