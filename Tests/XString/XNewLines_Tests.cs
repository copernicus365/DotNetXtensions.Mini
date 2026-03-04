namespace DNX.Test.Strings;

public class XNewLines_Tests
{
	const string text1_rn = "hello\r\nthere\r\nworld";
	const string text1_n = "hello\nthere\nworld";
	const string text1_r = "hello\rthere\rworld";
	const string text_mixed = "hello\r\nthere\nworld\rtest";
	const string text_empty_lines = "hello\r\n\r\nthere\n\nworld";
	const string text_whitespace = " hello \r\n  there  \r\n world ";

	static readonly GetLinesType[] _methodNames = [GetLinesType.GetLines, GetLinesType.EnumerateLinesLazy, GetLinesType.ForEachLine];

	enum GetLinesType { GetLines, EnumerateLinesLazy, ForEachLine }

	void _testAll(string text, string[] expected, bool trim = false, bool ignoreEmpty = false, bool unixOnly = false)
	{
		string[][] all = [
			text?.GetLines(trim: trim, ignoreEmpty: ignoreEmpty, unixOnly: unixOnly) ?? [],
			text?.GetLinesLazy(trim: trim, ignoreEmpty: ignoreEmpty, unixOnly: unixOnly).ToArray() ?? [],
#if NET9_0_OR_GREATER
			_foreachToArr(text, null, trim, ignoreEmpty),
#endif
		];

		for(int i = 0; i < all.Length; i++) {
			string[] lines = all[i];
			_assertEq(expected, lines, _methodNames[i]);
		}
	}

	string[] _foreachToArr(string text, int? max = null, bool trim = false, bool ignoreEmpty = false)
	{
		int mx = max ?? int.MaxValue;
		List<string> list = [];
		text?.ForEachLine(ln => { list.Add(ln); return list.Count < mx; }, trim: trim, ignoreEmpty: ignoreEmpty);
		return [.. list];
	}


	void _assertEq(string[] expected, string[] lines, GetLinesType methodNm)
	{
		Equal(expected.Length, lines.Length);
		True(lines.SequenceEqual(expected), $"{methodNm} produced incorrect results");
		//Equal(expected, lines);
	}

	[Fact]
	public void GetLines_Basic_CRLF()
		=> _testAll(text1_rn, ["hello", "there", "world"]);

	[Fact]
	public void GetLines_Basic_LF()
		=> _testAll(text1_n, ["hello", "there", "world"]);

	[Fact]
	public void GetLines_Basic_CR()
		=> _testAll(text1_r, ["hello", "there", "world"]);

	[Fact]
	public void GetLines_MixedLineEndings()
		=> _testAll(text_mixed, ["hello", "there", "world", "test"]);

	[Fact]
	public void GetLines_EmptyLines()
		=> _testAll(text_empty_lines, ["hello", "", "there", "", "world"]);


	[Fact]
	public void GetLines_IgnoreEmpty()
		=> _testAll(text_empty_lines, ["hello", "there", "world"], ignoreEmpty: true);

	[Fact]
	public void GetLines_Trim()
		=> _testAll(text_whitespace, ["hello", "there", "world"], trim: true);

	[Fact]
	public void GetLines_TrimAndIgnoreEmpty()
		=> _testAll(" hello \r\n\r\n  \r\n  there  \r\n\r\n world ", ["hello", "there", "world"], trim: true, ignoreEmpty: true);

	[Fact]
	public void GetLines_NullString()
		=> _testAll(null, []);

	[Fact]
	public void GetLines_EmptyString()
		=> _testAll(string.Empty, []);

	[Fact]
	public void GetLines_SingleLine()
		=> _testAll("hello", ["hello"]);

	[Fact]
	public void GetLines_TrailingNewLine()
		=> _testAll("hello\r\n", ["hello", ""]);

	[Fact]
	public void GetLines_TrailingNewLine_IgnoreEmpty()
		=> _testAll("hello\r\n", ["hello"], ignoreEmpty: true);

	[Fact]
	public void EnumerateLinesLazy_LazyEvaluation_Take()
	{
		// This test demonstrates TRUE lazy evaluation
		// Only the first 2 lines are processed, not all 3
		int processedCount = 0;
		var query = text1_rn.GetLinesLazy()
			.Select(line => { processedCount++; return line; })
			.Take(2);

		// At this point, nothing has been processed yet (lazy)
		Equal(0, processedCount);

		// Now enumerate and force evaluation
		string[] lines = [.. query];

		_assertEq(["hello", "there"], lines, GetLinesType.EnumerateLinesLazy);

		// Only 2 lines were processed, not 3!
		Equal(2, processedCount);
	}

	[Fact]
	public void EnumerateLinesLazy_First()
	{
		// Demonstrate that First() only processes the first line
		string firstLine = text1_rn.GetLinesLazy().First();
		Equal("hello", firstLine);
	}

	[Fact]
	public void EnumerateLinesLazy_WithLinqWhere()
	{
		// Demonstrate LINQ composition
		string[] lines = [.. text_mixed.GetLinesLazy().Where(line => line.StartsWith('t'))];

		_assertEq(["there", "test"], lines, GetLinesType.EnumerateLinesLazy);
	}


#if NET9_0_OR_GREATER

	[Fact]
	public void ForEachLine_StopEarly()
		=> _assertEq(["hello", "there"], _foreachToArr(text1_rn, max: 2), GetLinesType.ForEachLine);

#endif

}
