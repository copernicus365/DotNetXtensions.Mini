namespace DotNetXtensions;

public static partial class XString
{
	// maybe a mistake adding this ... hmmm

	/// <summary>Writes the string to <see cref="Console.WriteLine(string)"/> and returns it unchanged, for fluent/debug use.</summary>
	[DebuggerStepThrough]
	public static string Print(this string s)
	{
		Console.WriteLine(s);
		return s;
	}

	/// <summary>Writes the object to <see cref="Console.Write(object)"/>.</summary>
	[DebuggerStepThrough]
	public static void Print(this object obj)
		=> Console.Write(obj);
}
