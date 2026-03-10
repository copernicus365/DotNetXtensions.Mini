namespace DNX.Test.DateTimes;

/// <summary>
/// A little overkill on the AI tests for such a minor class. But they look good...
/// </summary>
public class XDateTimes_EpochOSConversions_Tests
{
	// Unix Epoch: 1970-01-01 00:00:00 UTC
	private const long UnixEpochTicks = 621355968000000000L;

	[Fact]
	public void TicksAtUnixEpoch_MatchesDateTimeUnixEpoch()
	{
		// Assert that our constant matches the .NET DateTime.UnixEpoch
		Equal(XDateTimes.TicksAtUnixEpoch, DateTime.UnixEpoch.Ticks);
		Equal(621355968000000000L, XDateTimes.TicksAtUnixEpoch);
	}

	[Fact]
	public void UnixMSToTicks_ZeroReturnsEpochTicks()
	{
		// Arrange
		long unixMs = 0;

		// Act
		long result = unixMs.UnixTimeMillisecondsToTicks();

		// Assert
		Equal(UnixEpochTicks, result);
		Equal(XDateTimes.TicksAtUnixEpoch, result);
	}

	[Fact]
	public void UnixMSToTicks_ConvertsCorrectly()
	{
		// Arrange: 1000ms = 1 second = 10,000,000 ticks
		long unixMs = 1000;

		// Act
		long result = unixMs.UnixTimeMillisecondsToTicks();

		// Assert
		Equal(UnixEpochTicks + 10_000_000, result);
	}

	[Fact]
	public void UnixMSToTicks_NormalConversion1()
	{
		// Arrange: Jan 1, 2024 00:00:00 UTC = 1704067200 seconds = 1704067200000 ms
		long unixMs = 1704067200000L;

		// Act
		long result = unixMs.UnixTimeMillisecondsToTicks();

		// Assert
		DateTime dt = new(result, DateTimeKind.Utc);

		Equal("1/1/2024 12:00:00 AM", dt.ToString());

		Equal(2024, dt.Year);
		Equal(1, dt.Month);
		Equal(1, dt.Day);
		Equal(0, dt.Hour);
		Equal(0, dt.Minute);
		Equal(0, dt.Second);
	}

	[Fact]
	public void UnixMSToTicks_PreEpochNegativeValueConverts()
	{
		// Arrange: -1000ms = 1 second before epoch
		long unixMs = -1000;

		// Act
		long result = unixMs.UnixTimeMillisecondsToTicks();

		// Assert
		Equal(UnixEpochTicks - 10_000_000, result);
		DateTime dt = new(result, DateTimeKind.Utc);

		Equal("12/31/1969 11:59:59 PM", dt.ToString());

		Equal(1969, dt.Year);
		Equal(12, dt.Month);
		Equal(31, dt.Day);
	}

	[Fact]
	public void UnixSecsToTicks_ZeroReturnsEpochTicks()
	{
		// Arrange
		long unixSec = 0;

		// Act
		long result = unixSec.UnixTimeSecondsToTicks();

		// Assert
		Equal(UnixEpochTicks, result);
		Equal(XDateTimes.TicksAtUnixEpoch, result);
	}

	[Fact]
	public void UnixSecsToTicks_ConvertsCorrectly()
	{
		// Arrange: 1 second = 10,000,000 ticks
		long unixSec = 1;

		// Act
		long result = unixSec.UnixTimeSecondsToTicks();

		// Assert
		Equal(UnixEpochTicks + 10_000_000, result);
	}

	[Fact]
	public void UnixSecsToTicks_HandlesLargeValues()
	{
		// Arrange: Jan 1, 2024 00:00:00 UTC = 1704067200 seconds
		long unixSec = 1704067200L;

		// Act
		long result = unixSec.UnixTimeSecondsToTicks();

		// Assert
		DateTime dt = new DateTime(result, DateTimeKind.Utc);
		Equal(2024, dt.Year);
		Equal(1, dt.Month);
		Equal(1, dt.Day);
	}

	[Fact]
	public void UnixSecsToTicks_HandlesNegativeValues()
	{
		// Arrange: -1 second before epoch
		long unixSec = -1;

		// Act
		long result = unixSec.UnixTimeSecondsToTicks();

		// Assert
		Equal(UnixEpochTicks - 10_000_000, result);
	}

	[Fact]
	public void UnixMSToDto_ZeroReturnsEpoch()
	{
		// Arrange
		long unixMs = 0;

		// Act
		DateTimeOffset result = unixMs.UnixTimeMillisecondsToDateTimeOffset();

		// Assert
		Equal(DateTime.UnixEpoch, result.DateTime);
		Equal(TimeSpan.Zero, result.Offset);
		Equal(1970, result.Year);
		Equal(1, result.Month);
		Equal(1, result.Day);
	}

	[Fact]
	public void UnixMSToDto_DefaultsToUtcOffset()
	{
		// Arrange
		long unixMs = 1000000;

		// Act
		DateTimeOffset result = unixMs.UnixTimeMillisecondsToDateTimeOffset();

		// Assert
		Equal(TimeSpan.Zero, result.Offset);
	}

	[Fact]
	public void UnixMSToDto_AcceptsCustomOffset()
	{
		// Arrange
		long unixMs = 0; // Epoch
		TimeSpan offset = TimeSpan.FromHours(5);

		// Act
		DateTimeOffset result = unixMs.UnixTimeMillisecondsToDateTimeOffset(offset);

		Equal("1/1/1970 12:00:00 AM +05:00", result.ToString());

		// Assert
		Equal(offset, result.Offset);
		Equal(1970, result.Year);
		Equal(1, result.Month);
		Equal(1, result.Day);
		Equal(0, result.Hour); // Local time component (ticks represent local, not UTC)
	}

	[Fact]
	public void UnixMSToDto_MatchesDotNetBuiltIn()
	{
		// Arrange: Jan 15, 2024 10:30:45.123 UTC
		long unixMs = 1705318245123L;

		// Act
		DateTimeOffset ourResult = unixMs.UnixTimeMillisecondsToDateTimeOffset();
		DateTimeOffset dotNetResult = DateTimeOffset.FromUnixTimeMilliseconds(unixMs);

		// Assert
		Equal(dotNetResult, ourResult);
		Equal(dotNetResult.DateTime, ourResult.DateTime);
		Equal(dotNetResult.Offset, ourResult.Offset);
	}

	[Fact]
	public void UnixSecsToDto_ZeroReturnsEpoch()
	{
		// Arrange
		long unixSec = 0;

		// Act
		DateTimeOffset result = unixSec.UnixTimeSecondsToDateTimeOffset();

		// Assert
		Equal(DateTime.UnixEpoch, result.DateTime);
		Equal(TimeSpan.Zero, result.Offset);
	}

	[Fact]
	public void UnixSecsToDto_OffsetByDefaultIsZero()
	{
		// Arrange
		long unixSec = 1000;

		// Act
		DateTimeOffset result = unixSec.UnixTimeSecondsToDateTimeOffset();

		// Assert
		Equal(TimeSpan.Zero, result.Offset);
	}

	[Fact]
	public void UnixSecsToDto_AcceptsCustomOffset()
	{
		// Arrange
		long unixSec = 0; // Epoch
		TimeSpan offset = TimeSpan.FromHours(-8);

		// Act
		DateTimeOffset result = unixSec.UnixTimeSecondsToDateTimeOffset(offset);

		Equal("1/1/1970 12:00:00 AM -08:00", result.ToString());

		// Assert
		Equal(offset, result.Offset);
		Equal(1970, result.Year); // Local time component
		Equal(1, result.Month);
		Equal(1, result.Day);
		Equal(0, result.Hour); // Local time component (ticks represent local, not UTC)
	}

	[Fact]
	public void UnixSecsToDto_MatchesDotNetBuiltIn()
	{
		// Arrange: Jan 15, 2024 10:30:45 UTC
		long unixSec = 1705318245L;

		// Act
		DateTimeOffset ourResult = unixSec.UnixTimeSecondsToDateTimeOffset();
		DateTimeOffset dotNetResult = DateTimeOffset.FromUnixTimeSeconds(unixSec);

		// Assert
		Equal(dotNetResult, ourResult);
		Equal(dotNetResult.DateTime, ourResult.DateTime);
		Equal(dotNetResult.Offset, ourResult.Offset);
	}

	[Fact]
	public void UnixSecsToDto_HandlesNegativeValues()
	{
		// Arrange: 1 day before epoch
		long unixSec = -86400L; // -1 day in seconds

		// Act
		DateTimeOffset result = unixSec.UnixTimeSecondsToDateTimeOffset();

		Equal("12/31/1969 12:00:00 AM +00:00", result.ToString());

		// Assert
		Equal(1969, result.Year);
		Equal(12, result.Month);
		Equal(31, result.Day);
	}

	[Fact]
	public void UnixSecsToDto_HandlesNegativeValues_WOffset()
	{
		// Arrange: 1 day before epoch
		long unixSec = -86400L; // -1 day in seconds
		TimeSpan offset = TimeSpan.FromHours(3);

		// Act
		DateTimeOffset result = unixSec.UnixTimeSecondsToDateTimeOffset(offset);

		Equal("12/31/1969 12:00:00 AM +03:00", result.ToString());

		// Assert
		Equal(1969, result.Year);
		Equal(12, result.Month);
		Equal(31, result.Day);
	}

	[Theory]
	[InlineData(0L, 1970, 1, 1)]
	[InlineData(1704067200L, 2024, 1, 1)]
	[InlineData(1735689600L, 2025, 1, 1)]
	[InlineData(946684800L, 2000, 1, 1)]
	public void UnixSecsToDto_VariousDates(long unixSec, int year, int month, int day)
	{
		// Act
		DateTimeOffset result = unixSec.UnixTimeSecondsToDateTimeOffset();

		// Assert
		Equal(year, result.Year);
		Equal(month, result.Month);
		Equal(day, result.Day);
	}

	[Theory]
	[InlineData(0L, 1970, 1, 1)]
	[InlineData(1704067200000L, 2024, 1, 1)]
	[InlineData(1735689600000L, 2025, 1, 1)]
	[InlineData(946684800000L, 2000, 1, 1)]
	public void UnixMSToDto_VariousDates(long unixMs, int year, int month, int day)
	{
		// Act
		DateTimeOffset result = unixMs.UnixTimeMillisecondsToDateTimeOffset();

		// Assert
		Equal(year, result.Year);
		Equal(month, result.Month);
		Equal(day, result.Day);
	}

	[Fact]
	public void Conversions_RoundTrip_PreservesOriginalValue()
	{
		// Arrange
		long originalUnixMs = 1705318245123L;

		// Act: Convert to DateTimeOffset and back
		DateTimeOffset dto = originalUnixMs.UnixTimeMillisecondsToDateTimeOffset();
		long roundTrippedMs = dto.ToUnixTimeMilliseconds();

		// Assert
		Equal(originalUnixMs, roundTrippedMs);
	}
}
