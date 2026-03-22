namespace DNX.Test.Linq;

public class XLinq_Empty_Tests
{
	#region --- EmptyIfNull ---

	[Fact]
	public void EmptyIfNull_Class_Null_ReturnsNewInstance()
	{
		List<int> list = null;
		var result = list.EmptyIfNull();
		NotNull(result);
		IsEmpty(result);
	}

	[Fact]
	public void EmptyIfNull_Class_NonNull_ReturnsSameReference()
	{
		var list = new List<int> { 1, 2 };
		Same(list, list.EmptyIfNull());
	}

	[Theory]
	[InlineData(null, "")]
	[InlineData("", "")]
	[InlineData("hello", "hello")]
	public void EmptyIfNull_String(string input, string expected)
		=> Equal(expected, input.EmptyIfNull());

	[Fact]
	public void EmptyIfNull_Array_Null_ReturnsEmptyArray()
	{
		int[] arr = null;
		int[] result = arr.EmptyIfNull();
		NotNull(result);
		IsEmpty(result);
	}

	[Fact]
	public void EmptyIfNull_Array_NonNull_ReturnsSameReference()
	{
		int[] arr = [1, 2, 3];
		Same(arr, arr.EmptyIfNull());
	}

	[Fact]
	public void EmptyIfNull_Enumerable_Null_ReturnsEmpty()
	{
		IEnumerable<int> seq = null;
		var result = seq.EmptyIfNull();
		NotNull(result);
		False(result.Any());
	}

	[Fact]
	public void EmptyIfNull_Enumerable_NonNull_ReturnsSameReference()
	{
		IEnumerable<int> seq = [1, 2, 3];
		Same(seq, seq.EmptyIfNull());
	}

	#endregion

	#region --- NullIfDefault ---

	[Theory]
	[InlineData(0, true)]
	[InlineData(5, false)]
	[InlineData(-1, false)]
	public void NullIfDefault_Int(int input, bool expectNull)
	{
		int? result = input.NullIfDefault();
		Equal(expectNull, result == null);
		if(!expectNull)
			Equal(input, result!.Value);
	}

	[Fact]
	public void NullIfDefault_Guid_Empty_ReturnsNull()
		=> Null(Guid.Empty.NullIfDefault());

	[Fact]
	public void NullIfDefault_Guid_NonEmpty_ReturnsSelf()
	{
		var g = Guid.NewGuid();
		Equal(g, g.NullIfDefault()!.Value);
	}

	#endregion

	#region --- ValueIfDefault ---

	[Theory]
	[InlineData(0, 99, 99)]
	[InlineData(5, 99, 5)]
	[InlineData(-1, 99, -1)]
	public void ValueIfDefault_Int(int input, int fallback, int expected)
		=> Equal(expected, input.ValueIfDefault(fallback));

	[Fact]
	public void ValueIfDefault_Guid_Default_ReturnsFallback()
	{
		var fallback = Guid.NewGuid();
		Equal(fallback, Guid.Empty.ValueIfDefault(fallback));
	}

	[Fact]
	public void ValueIfDefault_Guid_NonDefault_ReturnsOriginal()
	{
		var g = Guid.NewGuid();
		var fallback = Guid.NewGuid();
		NotEqual(g, fallback);
		Equal(g, g.ValueIfDefault(fallback));
	}

	#endregion

	#region --- LengthN ---

	[Fact]
	public void LengthN_Array_Null()
	{
		int[] arr = null;
		Equal(0, arr.LengthN());
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(5, 5)]
	public void LengthN_Array_Null_WDefaults(int defaultValue, int expected)
	{
		int[] arr = null;
		Equal(expected, arr.LengthN(defaultValue));
	}

	[Fact]
	public void LengthN_Array_Empty()
	{
		int[] arr = [];
		Equal(0, arr.LengthN());
	}

	[Fact]
	public void LengthN_Array_NonEmpty()
		=> Equal(3, new int[] { 1, 2, 3 }.LengthN());

	[Theory]
	[InlineData(null, 0, 0)]
	[InlineData(null, 5, 5)]
	[InlineData("", 0, 0)]
	[InlineData("hello", 0, 5)]
	public void LengthN_String(string s, int defaultValue, int expected)
		=> Equal(expected, s.LengthN(defaultValue));

	#endregion

	#region --- CountN ---

	[Fact]
	public void CountN_IList_Null()
	{
		IList<int> list = null;
		Equal(0, list.CountN());
		Equal(3, list.CountN(3));
	}

	[Fact]
	public void CountN_IList_NonNull()
	{
		IList<int> list = [1, 2, 3];
		List<int> list2 = [1, 2, 3];
		Equal(3, list.CountN());
	}

	[Fact]
	public void CountN_List_NonNull()
	{
		List<int> list = [1, 2, 3];
		Equal(3, list.CountN());
	}

	[Fact]
	public void CountN_ICollection_Null()
	{
		ICollection<string> coll = null;
		Equal(0, coll.CountN());
		Equal(7, coll.CountN(7));
	}
	//
	#endregion
}
