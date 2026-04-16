namespace DotNetXtensions;

/// <summary>
/// Extension methods for <see cref="string"/> — trim helpers, substring utilities, type parsing, and print utilities.
/// </summary>
public static partial class XString
{
	extension(string s)
	{
		/// <summary>True if the string is non-null, non-empty, and has leading or trailing whitespace.</summary>
		public bool IsTrimmable
			=> s != null && s.Length > 0 && (char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]));

		/// <summary>Trims the string only if it is needed. Value CAN be Null or Empty.</summary>
		[DebuggerStepThrough]
		public string TrimIfNeeded()
		{
			if(s == null || s.Length == 0) return s;
			if(char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]))
				return s.Trim();
			return s;
		}

		/// <summary>Trims the string; returns null if null, empty, or whitespace-only.
		/// *No* trimming happens if non-needed, in which case same string is returned</summary>
		[DebuggerStepThrough]
		public string TrimToNull()
		{
			if(s == null || s.Length == 0) return null;
			if(char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[^1]))
				s = s.Trim();
			return s.Length == 0 ? null : s;
		}

		/// <summary>Trims the string if it is not null, else returns null.</summary>
		[DebuggerStepThrough]
		public string TrimN() => s?.Trim();
	}
}
