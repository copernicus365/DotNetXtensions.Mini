namespace DNX.Test.Linq;

public class XLinq_Empty_Tests
{
	#region --- EmptyIfNull ---

	[Fact]
	public void EmptyIfNull_Class_Null_ReturnsNewInstance_FooB()
	{
		FooB foo = null;
		FooB result = foo.EmptyIfNull;
		result.Name = "Joe";
		NotNull(result);
		Equal(12, result.Id);
		Equal("Joe", result.Name);
	}

	[Fact]
	public void EmptyIfNull_Class_Null_ReturnsNewInstance_List()
	{
		List<int> list = null;
		List<int> result = list.EmptyIfNull;
		NotNull(result);
		IsEmpty(result);
	}

	[Fact]
	public void EmptyIfNull_Class_NonNull_ReturnsSameReference_List()
	{
		List<int> list = new List<int> { 1, 2 };
		Same(list, list.EmptyIfNull);
	}

	[Fact]
	public void EmptyIfNull_Class_NonNull_ReturnsSameReference_Foo()
	{
		FooB foo = new() { Id = 3, Name = "Anastasia" };
		Same(foo, foo.EmptyIfNull);
	}

	[Fact]
	public void EmptyIfNull_String_NullToEmpty()
	{
		string s = null;
		Equal("", s.EmptyIfNull);
	}

	[Fact]
	public void EmptyIfNull_String_EmptyToEmpty()
	{
		string s = "";
		Equal("", s.EmptyIfNull);
	}

	[Fact]
	public void EmptyIfNull_String_ValueStays()
	{
		string s = "hey!";
		Equal("hey!", s.EmptyIfNull);
	}

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

	[Fact]
	public void NullIfDefault_Int_0_ReturnsNull() => Null(0.NullIfDefault);

	[Fact]
	public void NullIfDefault_Int_Stays() => Equal(3, 3.NullIfDefault);

	[Fact]
	public void NullIfDefault_Int_Negative_Stays() => Equal(-4, -4.NullIfDefault);

	[Fact]
	public void NullIfDefault_Guid_Empty_ReturnsNull() => Null(Guid.Empty.NullIfDefault);

	[Fact]
	public void NullIfDefault_Guid_NonEmpty_ReturnsSelf()
	{
		Guid g = Guid.NewGuid();
		Equal(g, g.NullIfDefault!.Value);
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

	#region --- ValueOrDefault ---

	[Fact]
	public void ValueOrDefault_Null_ReturnsDefault()
	{
		int? val = null;
		Equal(0, val.ValueOrDefault);
	}

	[Fact]
	public void ValueOrDefault_Null_Vs_ValueOr()
	{
		int? val = null;
		Equal(0, val.ValueOrDefault);
		Equal(55, val.ValueOr(55));
	}

	[Fact]
	public void ValueOrDefault_Zero_ReturnsZero()
	{
		int? val = 0;
		Equal(0, val.ValueOrDefault);  // null-check only; 0 is not null, returns 0
	}

	[Fact]
	public void ValueOrDefault_WValue_ReturnsValue()
	{
		int? val = 5;
		Equal(5, val.ValueOrDefault);
	}

	#endregion

	#region --- ValueOr ---

	[Fact]
	public void ValueOr_Int_0_ReturnsAlt() => Equal(5, 0.ValueOr(5));

	[Fact]
	public void ValueOr_Int_WValue_Pos_ReturnsValue() => Equal(33, 33.ValueOr(5));

	[Fact]
	public void ValueOr_Int_WValue_Neg_ReturnsValue() => Equal(-22, -22.ValueOr(5));

	[Fact]
	public void ValueOr_Guid_Default_ReturnsFallback()
	{
		Guid fallback = Guid.NewGuid();
		Equal(fallback, Guid.Empty.ValueOr(fallback));
	}

	[Fact]
	public void ValueOr_Guid_WValue_ReturnsOriginal()
	{
		Guid g = Guid.NewGuid();
		Guid fallback = Guid.NewGuid();
		NotEqual(g, fallback);
		Equal(g, g.ValueOr(fallback));
	}

	// --- Nullable<T> ---

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
	public void ValueOr_Zero_ReturnsAltAsZero()
	{
		int? val = 0;
		Equal(0, val.ValueOr(0));
	}

	[Fact]
	public void ValueOr_WValue_ReturnsValue()
	{
		int? val = 5;
		Equal(5, val.ValueOr(99));
	}

	#endregion

	#region --- IsDefault / NotDefault ---

	[Fact]
	public void IsDefault_NotDefault_int_WValue()
	{
		int val = 3;
		False(val.IsDefault);
		True(val.NotDefault);
	}

	[Fact]
	public void IsDefault_NotDefault_int_0()
	{
		int val = 0;
		True(val.IsDefault);
		False(val.NotDefault);
	}

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
	public void IsDefault_Guid_Empty_True()
		=> True(Guid.Empty.IsDefault);

	[Fact]
	public void IsDefault_Guid_NonEmpty_False()
		=> False(Guid.NewGuid().IsDefault);

	[Fact]
	public void IsDefault_char_Empty_True()
		=> True(char.MinValue.IsDefault);

	[Fact]
	public void IsDefault_char_NonEmpty_False()
		=> False('a'.IsDefault);

	[Theory]
	[InlineData(null, true)]
	[InlineData(0, true)]   // default(int) is also "not set"
	[InlineData(5, false)]
	public void IsDefault_NotDefault_NullableInt(int? input, bool expected)
	{
		Equal(expected, input.IsDefault);
		Equal(expected, input.IsNullOrDefault);
		Equal(!expected, input.NotDefault);
		Equal(!expected, input.NotNullOrDefault);
	}

	#endregion

	#region --- LengthN ---

	[Fact]
	public void LengthN_Array_Null()
	{
		int[] arr = null;
		Equal(0, arr.LengthN);
	}

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

	#endregion

	#region --- CountN ---

	[Fact]
	public void CountN_IList_Null()
	{
		IList<int> list = null;
		Equal(0, list.CountN);
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

	class FooB { public int Id { get; set; } = 12; public string Name { get; set; } };
}
