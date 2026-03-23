namespace DNX.Test.Linq;

public class XLinq_Empty_Tests
{
	#region --- EmptyIfNull ---

	[Fact]
	public void EmptyIfNull_Class_Null_ReturnsNewInstance()
	{
		List<int> list = null;
		List<int> result = list.EmptyIfNull;
		NotNull(result);
		IsEmpty(result);
	}

	[Fact]
	public void EmptyIfNull_Class_NonNull_ReturnsSameReference()
	{
		List<int> list = new List<int> { 1, 2 };
		Same(list, list.EmptyIfNull);
	}

	[Theory]
	[InlineData(null, "")]
	[InlineData("", "")]
	[InlineData("hello", "hello")]
	public void EmptyIfNull_String(string input, string expected)
		=> Equal(expected, input.EmptyIfNull);

	[Fact]
	public void EmptyIfNull_Array_Null_ReturnsEmptyArray()
	{
		int[] arr = null;
		int[] result = arr.EmptyIfNull;
		NotNull(result);
		IsEmpty(result);
	}

	[Fact]
	public void EmptyIfNull_Array_NonNull_ReturnsSameReference()
	{
		int[] arr = [1, 2, 3];
		Same(arr, arr.EmptyIfNull);
	}

	[Fact]
	public void EmptyIfNull_Enumerable_Null_ReturnsEmpty()
	{
		IEnumerable<int> seq = null;
		IEnumerable<int> result = seq.EmptyIfNull;
		NotNull(result);
		False(result.Any());
	}

	[Fact]
	public void EmptyIfNull_Enumerable_NonNull_ReturnsSameReference()
	{
		IEnumerable<int> seq = [1, 2, 3];
		Same(seq, seq.EmptyIfNull);
	}

	#endregion

	#region --- NullIfDefault ---

	[Theory]
	[InlineData(0, true)]
	[InlineData(5, false)]
	[InlineData(-1, false)]
	public void NullIfDefault_Int(int input, bool expectNull)
	{
		int? result = input.NullIfDefault;
		Equal(expectNull, result == null);
		if(!expectNull)
			Equal(input, result!.Value);
	}

	[Fact]
	public void NullIfDefault_Guid_Empty_ReturnsNull()
		=> Null(Guid.Empty.NullIfDefault);

	[Fact]
	public void NullIfDefault_Guid_NonEmpty_ReturnsSelf()
	{
		Guid g = Guid.NewGuid();
		Equal(g, g.NullIfDefault!.Value);
	}

	#endregion

	#region --- ValueOr (struct) ---

	[Theory]
	[InlineData(0, 99, 99)]
	[InlineData(5, 99, 5)]
	[InlineData(-1, 99, -1)]
	public void ValueOr_Int(int input, int fallback, int expected)
		=> Equal(expected, input.ValueOr(fallback));

	[Fact]
	public void ValueOr_Guid_Default_ReturnsFallback()
	{
		Guid fallback = Guid.NewGuid();
		Equal(fallback, Guid.Empty.ValueOr(fallback));
	}

	[Fact]
	public void ValueOr_Guid_NonDefault_ReturnsOriginal()
	{
		Guid g = Guid.NewGuid();
		Guid fallback = Guid.NewGuid();
		NotEqual(g, fallback);
		Equal(g, g.ValueOr(fallback));
	}

	#endregion

	#region --- IsDefault / NotDefault (struct) ---

	[Theory]
	[InlineData(0, true)]
	[InlineData(5, false)]
	[InlineData(-1, false)]
	public void IsDefault_Int(int input, bool expected)
	{
		Equal(expected, input.IsDefault);
		Equal(!expected, input.NotDefault);
	}

	[Fact]
	public void IsDefault_Guid_Empty_ReturnsTrue()
		=> True(Guid.Empty.IsDefault);

	[Fact]
	public void IsDefault_Guid_NonEmpty_ReturnsFalse()
		=> False(Guid.NewGuid().IsDefault);

	[Fact]
	public void IsDefault_char_Empty_ReturnsTrue()
		=> True(char.MinValue.IsDefault);

	[Fact]
	public void IsDefault_char_NonEmpty_ReturnsFalse()
		=> False('a'.IsDefault);

	#endregion

	#region --- LengthN ---

	[Fact]
	public void LengthN_Array_Null()
	{
		int[] arr = null;
		Equal(0, arr.LengthN);
	}

	//[Theory]
	//[InlineData(0, 0)]
	//[InlineData(5, 5)]
	//public void LengthN_Array_Null_WDefaults(int defaultValue, int expected)
	//{
	//	int[] arr = null;
	//	Equal(expected, arr.LengthN(defaultValue));
	//}

	[Fact]
	public void LengthN_Array_Empty()
	{
		int[] arr = [];
		Equal(0, arr.LengthN);
	}

	[Fact]
	public void LengthN_Array_NonEmpty()
		=> Equal(3, new int[] { 1, 2, 3 }.LengthN);

	[Theory]
	[InlineData(null, 0)]
	[InlineData("", 0)]
	[InlineData("hello", 5)]
	public void LengthN_String(string s, int expected)
		=> Equal(expected, s.LengthN);

	//[Theory]
	//[InlineData(null, 0, 0)]
	//[InlineData(null, 5, 5)]
	//[InlineData("", 0, 0)]
	//[InlineData("hello", 0, 5)]
	//public void LengthN_String_WDefaults(string s, int defaultValue, int expected)
	//	=> Equal(expected, s.LengthN(defaultValue));

	#endregion

	#region --- CountN ---

	[Fact]
	public void CountN_IList_Null()
	{
		IList<int> list = null;
		Equal(0, list.CountN);
		////Equal(3, list.CountN(3));
	}

	[Fact]
	public void CountN_IList_NonNull()
	{
		IList<int> list = [1, 2, 3];
		Equal(3, list.CountN);
	}

	[Fact]
	public void CountN_List_NonNull()
	{
		List<int> list = [1, 2, 3];
		Equal(3, list.CountN);
	}

	[Fact]
	public void CountN_ICollection_Null()
	{
		ICollection<string> coll = null;
		Equal(0, coll.CountN);
		////Equal(7, coll.CountN(7));
	}

	[Fact]
	public void CountN_ICollection_NonNull()
	{
		ICollection<int> coll = new List<int> { 10, 20 };
		Equal(2, coll.CountN);
	}

	#endregion

	#region --- IsEmpty / NotEmpty (string) ---

	[Theory]
	[InlineData(null, true)]
	[InlineData("", true)]
	[InlineData("hellom", false)]
	public void IsEmpty_String(string s, bool isEmpty)
		=> Equal(isEmpty, s.IsEmpty);

	[Theory]
	[InlineData(null, false)]
	[InlineData("", false)]
	[InlineData("hello", true)]
	public void NotEmpty_String(string s, bool isEmpty)
		=> Equal(isEmpty, s.NotEmpty);

	[Theory]
	[InlineData(null, true)]
	[InlineData("", true)]
	[InlineData("  ", true)]
	[InlineData("hello", false)]
	public void IsEmptyOrWhiteSpace_String(string s, bool isEmpty)
		=> Equal(isEmpty, s.IsEmptyOrWhiteSpace);

	[Theory]
	[InlineData(null, null)]
	[InlineData("", null)]
	[InlineData("hello", "hello")]
	public void NullIfEmpty_String(string s, string expected)
		=> Equal(expected, s.NullIfEmpty);

	#endregion

	#region --- IsEmpty / NotEmpty (Array) ---

	[Fact]
	public void IsEmpty_Array_Null()
	{
		int[] arr = null;
		True(arr.IsEmpty);
		True(arr.IsNullOrEmpty);
		False(arr.NotEmpty);
		False(arr.NotNullOrEmpty);
	}

	[Fact]
	public void IsEmpty_Array_Empty()
	{
		int[] arr = [];
		True(arr.IsEmpty);
		True(arr.IsNullOrEmpty);
		False(arr.NotEmpty);
		False(arr.NotNullOrEmpty);
	}

	[Fact]
	public void IsEmpty_Array_NonEmpty()
	{
		int[] arr = [1, 2, 3];
		False(arr.IsEmpty);
		False(arr.IsNullOrEmpty);
		True(arr.NotEmpty);
		True(arr.NotNullOrEmpty);
	}

	#endregion

	#region --- IsEmpty / NotEmpty (ICollection) ---

	[Fact]
	public void IsEmpty_ICollection_Null()
	{
		ICollection<int> coll = null;
		True(coll.IsEmpty);
		True(coll.IsNullOrEmpty);
		False(coll.NotEmpty);
		False(coll.NotNullOrEmpty);
	}

	[Fact]
	public void IsEmpty_ICollection_Empty()
	{
		ICollection<int> coll = new List<int>();
		True(coll.IsEmpty);
		True(coll.IsNullOrEmpty);
		False(coll.NotEmpty);
		False(coll.NotNullOrEmpty);
	}

	[Fact]
	public void IsEmpty_ICollection_NonEmpty()
	{
		ICollection<int> coll = new List<int> { 1, 2 };
		False(coll.IsEmpty);
		False(coll.IsNullOrEmpty);
		True(coll.NotEmpty);
		True(coll.NotNullOrEmpty);
	}

	#endregion

	#region --- Nullable<T>: IsDefault / NotDefault / ValueOrDefault / ValueOr ---

	[Theory]
	[InlineData(null, true)]
	[InlineData(0, true)]   // default(int) is also "not set"
	[InlineData(5, false)]
	public void IsDefault_NullableInt(int? input, bool expected)
	{
		Equal(expected, input.IsDefault);
		Equal(expected, input.IsNullOrDefault);
	}

	[Theory]
	[InlineData(null, false)]
	[InlineData(0, false)]
	[InlineData(5, true)]
	public void NotDefault_NullableInt(int? input, bool expected)
	{
		Equal(expected, input.NotDefault);
		Equal(expected, input.NotNullOrDefault);
	}

	[Fact]
	public void ValueOrDefault_Null_ReturnsDefault()
	{
		int? val = null;
		Equal(0, val.ValueOrDefault);
	}

	[Fact]
	public void ValueOrDefault_Zero_ReturnsZero()
	{
		int? val = 0;
		Equal(0, val.ValueOrDefault);  // null-check only; 0 is not null, returns 0
	}

	[Fact]
	public void ValueOrDefault_NonDefault_ReturnsValue()
	{
		int? val = 5;
		Equal(5, val.ValueOrDefault);
	}

	[Fact]
	public void ValueOr_Null_ReturnsAlt()
	{
		int? val = null;
		Equal(99, val.ValueOr(99));
	}

	[Fact]
	public void ValueOr_Zero_ReturnsAlt()
	{
		int? val = 0;
		Equal(99, val.ValueOr(99));  // 0 == default, treated as "not set"
	}

	[Fact]
	public void ValueOr_NonDefault_ReturnsValue()
	{
		int? val = 5;
		Equal(5, val.ValueOr(99));
	}

	[Theory]
	[InlineData(null, true)]
	[InlineData(0, true)]
	[InlineData(5, false)]
	public void NullIfDefault_NullableInt(int? input, bool expectNull)
	{
		int? result = input.NullIfDefault;
		Equal(expectNull, result == null);
		if(!expectNull)
			Equal(input, result!.Value);
	}

	#endregion
}
