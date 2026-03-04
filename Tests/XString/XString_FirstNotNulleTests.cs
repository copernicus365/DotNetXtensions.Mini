namespace DNX.Test.Strings;

public class XString_FirstNotNulleTests
{
	[Theory]
	[InlineData(0, "aaa", null, null)]
	[InlineData(0, "aaa", "", null)]
	[InlineData(0, "aaa", "bbb", null)]
	[InlineData(0, "aaa", "", "ccc")]
	[InlineData(0, "aaa", null, "ccc")]
	[InlineData(0, " ", null, "ccc")]
	[InlineData(0, "\t", null, "ccc")]

	[InlineData(1, null, "bbb", null)]
	[InlineData(1, "", "bbb", null)]
	[InlineData(1, "", "\n", null)]

	[InlineData(2, "", null, "ccc")]
	[InlineData(2, "", "", "ccc")]

	[InlineData(-1, "", "", "")]
	[InlineData(-1, null, null, null)]
	[InlineData(-1, null, null, "")]
	[InlineData(-1, null, "", "")]
	public void FirstNotNulle(int match, string val1, string val2, string val3)
	{
		if(match.NotInRange(-1, 2)) throw new ArgumentOutOfRangeException(nameof(match));

		string res = val1.FirstNotNulle(val2, val3);
		
		if(match < 0) {
			Null(res);
			True(val1.IsNulle());
			True(val2.IsNulle());
			True(val3.IsNulle());
			return;
		}

		string[] vals = [val1, val2, val3];
		string exp = vals[match];
		Equal(exp, res);

		for(int i = 0; i < match; i++)
			True(vals[i].IsNulle());
	}
}
