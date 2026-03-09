namespace DNX.Test.DateTimes;

public class XDateTimes_OffsetConversions_Tests
{
	DateTimeOffset time1(int hr, int min, int offset) => new(2024, 1, 15, hr, min, 0, ts(offset));
	DateTimeOffset time1(int hr, int offset) => time1(hr, min: 0, offset: offset);
	DateTimeOffset time1() => time1(hr: 10, min: 0, offset: 0);


	DateTime time1d(int hr, int min, DateTimeKind kind = default)
		=> new(2024, 1, 15, hr, min, 0, kind);

	DateTime time1d(DateTimeKind kind = default)
		=> time1d(10, 0, kind);

	TimeSpan ts(int hours) => hours == 0 ? TimeSpan.Zero : TimeSpan.FromHours(hours);

	TimeZoneInfo getTZI(string tzi)
		=> TimeZoneInfo.FindSystemTimeZoneById(tzi);


	[Fact]
	public void ToOffsetSameUtc_PreservesUtcTime_ChangesLocalTime()
	{
		// Arrange: 15:00 local with +05:00 offset (UTC: 10:00)
		DateTimeOffset dt = time1(hr: 15, offset: 5);

		Equal("1/15/2024 3:00:00 PM +05:00", dt.ToString());

		// Act: Change to -08:00 offset
		DateTimeOffset result = dt.ToOffsetSameUtc(TimeSpan.FromHours(-8));

		Equal("1/15/2024 2:00:00 AM -08:00", result.ToString());

		// Assert: UTC stays 10:00, local becomes 02:00
		Equal(time1(hr: 2, offset: -8), result);
		Equal(dt.UtcDateTime, result.UtcDateTime); // Same UTC
		NotEqual(dt.DateTime, result.DateTime); // Different local
		Equal(dt, result); // Equal because UTC is same
	}

	[Fact]
	public void ToOffsetSameUtc_WithTimeZoneInfo_CalculatesOffsetCorrectly()
	{
		// Arrange
		DateTimeOffset dt = time1();
		TimeZoneInfo eastCoastTz = getTZI("Eastern Standard Time");

		// Act
		DateTimeOffset result = dt.ToOffsetSameUtc(eastCoastTz);

		// Assert: UTC 10:00 should become 05:00 EST (UTC-5 in winter)
		Equal(dt.UtcDateTime, result.UtcDateTime);
		Equal(TimeSpan.FromHours(-5), result.Offset);
		Equal(time1d(hr: 5, min: 0), result.DateTime);
	}

	[Fact]
	public void ToOffsetSameUtc_DiscardsOriginalOffset()
	{
		// Arrange: Start with +03:00 offset
		DateTimeOffset dt = new DateTimeOffset(2024, 6, 15, 14, 30, 0, TimeSpan.FromHours(3));

		// Act: Change to +09:00
		DateTimeOffset result = dt.ToOffsetSameUtc(TimeSpan.FromHours(9));

		// Assert: Original +03:00 is completely discarded
		Equal(TimeSpan.FromHours(9), result.Offset);
		Equal(dt.UtcDateTime, result.UtcDateTime);
	}

	[Fact]
	public void ToDateTimeOffset_TreatsInputAsLocalTime_PreservesTicks()
	{
		// Arrange: DateTime with UTC Kind (which will be ignored)
		DateTime dt = time1d(hr: 10, min: 30, DateTimeKind.Utc);

		int offset = 5;

		// Act
		DateTimeOffset result = dt.ToDateTimeOffset(TimeSpan.FromHours(offset));

		// Assert: Ticks preserved (treated as local), Kind ignored

		DateTimeOffset exp = time1(hr: 10, min: 30, offset: offset);

		Equal(exp, result);
		Equal(dt.Ticks, result.DateTime.Ticks);
		// UTC should be: 10:30 - 5 hours = 05:30
		Equal(time1d(hr: 5, min: 30), result.UtcDateTime);
	}

	[Fact]
	public void ToDateTimeOffsetFromUtc_TreatsInputAsUtc_AdjustsLocalByOffset()
	{
		// Arrange: DateTime representing 10:00 UTC
		DateTime dt = time1d(DateTimeKind.Local); // Kind ignored
		TimeSpan offset = TimeSpan.FromHours(5);

		// Act
		DateTimeOffset result = dt.ToDateTimeOffsetFromUtc(offset);

		// Assert: Local time adjusted to 15:00 (10:00 + 5 hours)
		Equal(new DateTimeOffset(time1d(hr: 15, min: 0), offset), result);
		Equal(time1d(), result.UtcDateTime);
	}

	[Fact]
	public void ToDateTimeOffset_WithTimeZoneInfo_IgnoresDateTimeKind()
	{
		// Arrange: DateTime with UTC kind
		DateTime dt = time1d(hr: 14, min: 0, DateTimeKind.Utc);
		TimeZoneInfo pst = getTZI("Pacific Standard Time");

		// Act: Should not throw even though Kind is UTC
		DateTimeOffset result = dt.ToDateTimeOffset(pst);

		Equal("1/15/2024 2:00:00 PM -08:00", result.ToString());

		// Assert: Kind ignored, treated as local
		Equal(time1d(hr: 14, min: 0), result.DateTime);
	}

	[Fact]
	public void ToDateTimeOffsetFromUtc_WithTimeZoneInfo_AdjustsCorrectly()
	{
		// Arrange: 10:00 UTC
		DateTime dt = time1d();
		TimeZoneInfo centralTz = getTZI("Central Standard Time");

		// Act
		DateTimeOffset result = dt.ToDateTimeOffsetFromUtc(centralTz);

		// Assert: Should be 04:00 CST (UTC-6 in winter)
		Equal(time1d(hr: 4, min: 0), result.DateTime);
		Equal(TimeSpan.FromHours(-6), result.Offset);
		Equal(time1d(), result.UtcDateTime);
	}

	[Fact]
	public void ToUnspecifiedKindIfUtc_ConvertsUtcToUnspecified()
	{
		// Arrange
		DateTime dt = time1d(DateTimeKind.Utc);

		// Act
		DateTime result = dt.ToUnspecifiedKindIfUtc();

		// Assert
		Equal(DateTimeKind.Unspecified, result.Kind);
		Equal(dt.Ticks, result.Ticks);
	}


	[Fact]
	public void ToUnspecifiedKindIfUtc_LeavesLocalAndUnspecifiedUnchanged()
	{
		// Arrange
		DateTime dtLocal = time1d(DateTimeKind.Local);
		DateTime dtUnspec = time1d(DateTimeKind.Unspecified);

		// Act
		DateTime resultLocal = dtLocal.ToUnspecifiedKindIfUtc();
		DateTime resultUnspec = dtUnspec.ToUnspecifiedKindIfUtc();

		// Assert
		Equal(DateTimeKind.Local, resultLocal.Kind);
		Equal(DateTimeKind.Unspecified, resultUnspec.Kind);
		Equal(dtLocal, resultLocal); // Should return same instance
		Equal(dtUnspec, resultUnspec);
	}

	[Fact]
	public void ToOffsetSameUtc_MinValue_ReturnsMinValue()
	{
		// Arrange
		DateTimeOffset dt = DateTimeOffset.MinValue;

		// Act
		DateTimeOffset result = dt.ToOffsetSameUtc(TimeSpan.FromHours(5));

		// Assert
		Equal(DateTimeOffset.MinValue, result);
	}

	[Fact]
	public void ToDateTimeOffset_MinValue_ReturnsMinValue()
	{
		// Arrange
		DateTime dt = DateTime.MinValue;

		// Act
		DateTimeOffset result = dt.ToDateTimeOffset(TimeSpan.FromHours(5));

		// Assert
		Equal(DateTimeOffset.MinValue, result);
	}

	[Fact]
	public void ToOffsetSameUtc_MaxValue_ReturnsMaxValue()
	{
		// Arrange: Close to max that would overflow when offset added
		DateTimeOffset dt = new DateTimeOffset(DateTime.MaxValue.AddHours(-1), TimeSpan.Zero);

		// Act: Adding large positive offset should hit max
		DateTimeOffset result = dt.ToOffsetSameUtc(TimeSpan.FromHours(14));

		// Assert
		Equal(DateTimeOffset.MaxValue, result);
	}

	[Fact]
	public void ToDateTimeOffsetFromUtc_MaxValue_ReturnsMaxValue()
	{
		// Arrange: DateTime near max
		DateTime dt = DateTime.MaxValue.AddHours(-1);

		// Act: Adding offset should hit max
		DateTimeOffset result = dt.ToDateTimeOffsetFromUtc(TimeSpan.FromHours(14));

		// Assert
		Equal(DateTimeOffset.MaxValue, result);
	}

	[Fact]
	public void ToOffsetSameUtc_InvalidOffset_ThrowsArgumentOutOfRangeException()
	{
		// Arrange
		DateTimeOffset dt = time1();

		// Act & Assert: offset too negative
		Throws<ArgumentOutOfRangeException>(() =>
			dt.ToOffsetSameUtc(TimeSpan.FromHours(-15)));

		// Act & Assert: offset too positive
		Throws<ArgumentOutOfRangeException>(() =>
			dt.ToOffsetSameUtc(TimeSpan.FromHours(15)));
	}

	[Fact]
	public void ToDateTimeOffset_InvalidOffset_ThrowsArgumentOutOfRangeException()
	{
		// Arrange
		DateTime dt = time1d();

		// Act & Assert
		Throws<ArgumentOutOfRangeException>(() =>
			dt.ToDateTimeOffset(TimeSpan.FromHours(-15)));
		Throws<ArgumentOutOfRangeException>(() =>
			dt.ToDateTimeOffsetFromUtc(TimeSpan.FromHours(15)));
	}

	[Fact]
	public void ToOffsetSameUtc_NegativeTicksResult_ReturnsMinValue()
	{
		// Arrange: Very early date with large negative offset
		DateTimeOffset dt = DateTimeOffset.MinValue.AddHours(1);

		//new DateTimeOffset(new DateTime(100, 1, 1), TimeSpan.Zero);

		// Act: Large negative offset should result in negative ticks
		DateTimeOffset result = dt.ToOffsetSameUtc(TimeSpan.FromHours(-14));

		// Assert
		Equal(DateTimeOffset.MinValue, result);
	}

	[Fact]
	public void FrameworkToOffset_KeepsUtcFixed_ChangesLocal_SameAsToOffsetSameUtc()
	{
		// This test demonstrates that framework's ToOffset has the SAME behavior as ToOffsetSameUtc
		// Arrange: 10:00 local with +00:00 offset (UTC: 10:00)
		DateTimeOffset dt = time1();

		Equal("1/15/2024 10:00:00 AM +00:00", dt.ToString());

		// Act: Framework's ToOffset
		DateTimeOffset result = dt.ToOffset(TimeSpan.FromHours(5));
		Equal("1/15/2024 3:00:00 PM +05:00", result.ToString());

		// Assert: UTC stays 10:00, local becomes 15:00 (to maintain same UTC instant)
		DateTimeOffset expected = time1(hr: 15, offset: 5);
		Equal(expected, result);
		Equal(expected.ToString(), result.ToString());

		Equal(dt.UtcDateTime, result.UtcDateTime); // Same UTC (10:00)
		NotEqual(dt.DateTime, result.DateTime); // Different local (10:00 → 15:00)
		Equal(dt, result); // Equal because same UTC instant
	}
}
