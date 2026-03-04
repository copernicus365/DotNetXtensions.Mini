namespace DotNetXtensions;

public static partial class XString
{
	public static bool ContainsN(this string str, string value, StringComparison comparison)
	{
		if(str == null || value.IsNulle()) return false;
		int idx = str.IndexOf(value, comparison);
		return idx >= 0;
	}

	public static bool ContainsN(this string str, string value)
		=> str != null && str.Contains(value);

	public static bool ContainsIgnoreCase(this string s, string value)
		=> s != null && value != null && s.Contains(value, StringComparison.OrdinalIgnoreCase);
}
