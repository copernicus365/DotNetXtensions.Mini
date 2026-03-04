namespace DotNetXtensions;

public static class XChar
{
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
}
