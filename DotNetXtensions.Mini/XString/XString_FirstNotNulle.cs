namespace DotNetXtensions;

public static partial class XString
{
	/// <summary>
	/// Returns first input string that is not null or empty. If all are null or empty, returns null.
	/// </summary>
	/// <param name="value1">Value 1.</param>
	/// <param name="value2">Value 2.</param>
	/// <param name="value3">Value 3.</param>
	[DebuggerStepThrough]
	public static string FirstNotNulle(this string value1, string value2, string value3 = null)
	{
		if(value1 != null && value1.Length > 0)
			return value1;

		if(value2 != null && value2.Length > 0)
			return value2;

		if(value3 != null && value3.Length > 0)
			return value3;

		return null;
	}
}
