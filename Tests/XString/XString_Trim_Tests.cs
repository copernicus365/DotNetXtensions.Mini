namespace DNX.Test.Strings;

public class XString_Trim_Tests
{
	#region --- IsTrimmable ---

	[Theory]
	[InlineData(null, false)]
	[InlineData("", false)]
	[InlineData("hello", false)]
	[InlineData(" hello", true)]
	[InlineData("hello ", true)]
	[InlineData(" hello ", true)]
	[InlineData(" ", true)]
	public void IsTrimmable(string input, bool expected)
		=> Equal(expected, input.IsTrimmable);

	#endregion

	#region --- TrimToNull ---

	[Theory]
	[InlineData(null, null)]
	[InlineData("", null)]
	[InlineData(" ", null)]
	[InlineData("hello", "hello")]
	[InlineData(" hello", "hello")]
	[InlineData("hello ", "hello")]
	[InlineData(" hello ", "hello")]
	public void TrimToNull(string input, string expected)
		=> Equal(expected, input.TrimToNull());

	#endregion

	#region --- TrimIfNeeded ---

	[Theory]
	[InlineData(null, null)]
	[InlineData("", "")]
	[InlineData("hello", "hello")]
	[InlineData(" hello", "hello")]
	[InlineData("\thello", "hello")]
	[InlineData("\nhello", "hello")]
	[InlineData("hello ", "hello")]
	[InlineData(" hello ", "hello")]
	[InlineData(" ", "")]
	public void TrimIfNeeded(string input, string expected)
		=> Equal(expected, input.TrimIfNeeded());

	[Fact]
	public void TrimIfNeeded_NoTrimNeeded_ReturnsSameReference()
	{
		string s = "hello";
		Same(s, s.TrimIfNeeded());
	}

	#endregion

	#region --- TrimN ---

	[Theory]
	[InlineData(null, null)]
	[InlineData("", "")]
	[InlineData("hello", "hello")]
	[InlineData(" hello ", "hello")]
	[InlineData("\thello ", "hello")]
	[InlineData("\nhello ", "hello")]
	[InlineData(" ", "")]
	public void TrimN(string input, string expected)
		=> Equal(expected, input.TrimN());

	#endregion
}
