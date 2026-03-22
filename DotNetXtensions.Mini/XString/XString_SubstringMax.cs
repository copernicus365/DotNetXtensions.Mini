namespace DotNetXtensions;

public static partial class XString
{
	public static string SubstringMax(this string str, int maxLength, string ellipsis = null, bool tryBreakOnWord = false)
		=> SubstringMax(str, 0, maxLength, ellipsis: ellipsis, tryBreakOnWord: tryBreakOnWord);

	/// <summary>
	/// Returns a substring of the input string where instead of specifying
	/// the exact length of the return string (which in .NET's string.Substring
	/// cannot be specified out of range), one specifies a maxLength, meaning
	/// maxLength can be out of range, in which case the substring from index
	/// to the end of the string is returned.
	/// <para />
	/// If the string is NULL or EMPTY, the same is immediately returned, NO exceptions
	/// (Null or OutOfRange) will be thrown.
	/// <para/>
	/// This nicely solves the problem when one simply wants the first n length
	/// of characters from a string, but without having to write a bunch of
	/// code to make sure they do not go out of range in case, for instance, the string was shorter
	/// than expected.
	/// </summary>
	/// <param name="str">String</param>
	/// <param name="index">Start Index</param>
	/// <param name="maxLength">Maximum length of the return substring.</param>
	/// <param name="ellipsis">...</param>
	/// <param name="tryBreakOnWord"></param>
	public static string SubstringMax(this string str, int index, int maxLength, string ellipsis = null, bool tryBreakOnWord = false)
	{
		const int maxWordBreakSize = 15;
		const int minCheckWordBreak = 7;

		if(str == null || str.Length == 0) return str;

		int strLen = str.Length;
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, strLen);

		if(index == 0 && strLen <= maxLength)
			return str;

		int finalLen = strLen - index;
		bool useEllipsis = ellipsis.NotEmpty;

		if(maxLength < finalLen)
			finalLen = maxLength;
		else
			useEllipsis = false; // was true up to here if wasn't nulle

		// WOULD BE MORE PERFORMANT TO DO THE WORD-BREAK SEARCH HERE,
		// NOT NEED AN EXTRA STRING ALLOC. WANNA DO IT?! GO AHEAD, MAKE MY DAY!

		int postIdx = index + finalLen;
		string result = str.Substring(index, finalLen);

		// WORD-BREAK SEARCH
		if(tryBreakOnWord && postIdx < strLen && char.IsLetterOrDigit(str, postIdx) && result.Length > minCheckWordBreak) {
			int i = 0;
			int x = result.Length - 1;
			for(; i < maxWordBreakSize && x >= minCheckWordBreak; i++, x--) {
				if(char.IsWhiteSpace(result[x]))
					break;
			}
			if(i > 0 && i < maxWordBreakSize && x >= minCheckWordBreak)
				result = result.Substring(0, x + 1);
		}

		if(useEllipsis)
			result += ellipsis;

		return result;
	}
}
