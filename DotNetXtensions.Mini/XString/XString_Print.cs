namespace DotNetXtensions;

public static partial class XString
{
	// maybe a mistake adding this ... hmmm

	[DebuggerStepThrough]
	public static string Print(this string s)
	{
		Console.WriteLine(s);
		return s;
	}

	[DebuggerStepThrough]
	public static void Print(this object obj)
		=> Console.Write(obj);
}
