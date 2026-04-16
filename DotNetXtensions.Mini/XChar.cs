namespace DotNetXtensions;

/// <summary>
/// Extension methods for <see cref="char"/> — ASCII range checks and digit-to-int conversion.
/// </summary>
public static class XChar
{
	/// <summary>
	/// Indicates whether the char is an ascii digit (0-9 only).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiDigit(this char c)
		=> c < 58 && c > 47;

	/// <summary>
	/// Indicates whether the char is an ascii letter (a-z or A-Z only).
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

	/// <summary>Indicates whether the char is whitespace (delegates to <see cref="char.IsWhiteSpace(char)"/>).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsWhitespace(this char c)
		=> char.IsWhiteSpace(c);

	/// <summary>Indicates whether the char is uppercase (delegates to <see cref="char.IsUpper(char)"/>).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsUpper(this char c)
		=> char.IsUpper(c);

	/// <summary>Indicates whether the char is lowercase (delegates to <see cref="char.IsLower(char)"/>).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLower(this char c)
		=> char.IsLower(c);

	/// <summary>Indicates whether the char is a number (delegates to <see cref="char.IsNumber(char)"/>).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNumber(this char c)
		=> char.IsNumber(c);

	/// <summary>Converts an ascii digit char ('0'–'9') to its integer value. No validation — caller must ensure the char is a digit.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ToInt(this char c)
		=> c - '0';
}
