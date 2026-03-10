namespace DNX.Test.DateTimes;

public class XDateTimes_Round_Tests
{
	// Base reference time in ticks: 2025-02-14 12:00:00
	const long Base = 638441280000000000L;  // = new DateTime(2025, 2, 14, 12, 0, 0).Ticks

	// Common time intervals in ticks
	const long SecTicks = 10_000_000L;      // 1 second
	const long MinTicks = 600_000_000L;     // 1 minute (60 seconds)
	const long HourTicks = 36_000_000_000L; // 1 hour (60 minutes)

	// Tick calculation helpers
	long secTicks(int secs) => secs * SecTicks;
	long minTicks(int mins) => mins * MinTicks;
	long hourTicks(int hours) => hours * HourTicks;

	// Returns raw ticks from Base (use named params for clarity)
	long timeTicks(int hours = 0, int mins = 0, int secs = 0)
		=> Base + hourTicks(hours) + minTicks(mins) + secTicks(secs);

	// --- Low-level helpers ---
	DateTime dt(long ticks, DateTimeKind kind = DateTimeKind.Unspecified) => new(ticks, kind);
	DateTimeOffset dto(long ticks, int offsetHours = 0) => new(ticks, TimeSpan.FromHours(offsetHours));

	// --- Friendly time helpers ---
	DateTime time() => dt(Base);
	DateTime time(int mins, DateTimeKind kind = DateTimeKind.Unspecified) => dt(timeTicks(mins: mins), kind);
	DateTime timeSec(int secs, DateTimeKind kind = DateTimeKind.Unspecified) => dt(timeTicks(secs: secs), kind);
	DateTime timeHour(int hours, DateTimeKind kind = DateTimeKind.Unspecified) => dt(timeTicks(hours: hours), kind);

	DateTimeOffset timeO(int mins, int offset) => dto(timeTicks(mins: mins), offset);
	DateTimeOffset timeSecO(int secs, int offset) => dto(timeTicks(secs: secs), offset);
	DateTimeOffset timeHourO(int hours, int offset) => dto(timeTicks(hours: hours), offset);

	// --- Test Helpers (return values, no assertions) ---

	DateTime _roundDT(int startSecs, int intervalMins, DateTimeKind kind = DateTimeKind.Unspecified)
	{
		long startTicks = timeTicks(secs: startSecs);
		DateTime result = dt(startTicks, kind).Round(TimeSpan.FromMinutes(intervalMins));
		return result;
	}

	DateTimeOffset _roundDTO(int startSecs, int intervalMins, int offset)
	{
		long startTicks = timeTicks(secs: startSecs);
		DateTimeOffset result = dto(startTicks, offsetHours: offset).Round(TimeSpan.FromMinutes(intervalMins));
		return result;
	}

	DateTime _roundUpDT(long startTicks, TimeSpan interval, DateTimeKind kind = DateTimeKind.Unspecified)
	{
		DateTime result = dt(startTicks, kind).RoundUp(interval);
		return result;
	}

	DateTimeOffset _roundUpDTO(long startTicks, TimeSpan interval, int offset)
	{
		DateTimeOffset result = dto(startTicks, offsetHours: offset).RoundUp(interval);
		return result;
	}

	DateTime _roundDownDT(long startTicks, TimeSpan interval, DateTimeKind kind = DateTimeKind.Unspecified)
	{
		DateTime result = dt(startTicks, kind).RoundDown(interval);
		return result;
	}

	DateTimeOffset _roundDownDTO(long startTicks, TimeSpan interval, int offset)
	{
		DateTimeOffset result = dto(startTicks, offsetHours: offset).RoundDown(interval);
		return result;
	}

	// --- Round Tests ---

	[Fact]
	public void Round_EvenInterval_MidpointRoundsUp()
		=> Equal(time(1), _roundDT(30, 1));

	[Fact]
	public void Round_EvenInterval_BelowMidpointRoundsDown()
		=> Equal(time(), _roundDT(29, 1));

	[Fact]
	public void Round_EvenInterval_AboveMidpointRoundsUp()
		=> Equal(time(1), _roundDT(31, 1));

	[Fact]
	public void Round_OddInterval_BelowMidpointRoundsDown()
	{
		// Arrange: 11 ticks interval (odd), midpoint at 5.5 ticks
		// 100 % 11 = 1 (delta = 1)
		long startTicks = 100;
		var interval = TimeSpan.FromTicks(11);

		// Act
		DateTime result = dt(startTicks).Round(interval);

		// Assert: delta=1, 1*2=2 < 11, rounds DOWN to 99
		Equal(dt(99), result);
	}

	[Fact]
	public void Round_OddInterval_AtFloorOfMidpointRoundsDown()
	{
		// Arrange: delta = 5, midpoint = 5.5, so 5 should round DOWN
		// 104 % 11 = 5
		long startTicks = 104;
		var interval = TimeSpan.FromTicks(11);

		// Act
		DateTime result = dt(startTicks).Round(interval);

		// Assert: delta=5, 5*2=10 < 11, rounds DOWN to 99
		Equal(dt(99), result);
	}

	[Fact]
	public void Round_OddInterval_AboveMidpointRoundsUp()
	{
		// Arrange: delta = 6, midpoint = 5.5, so 6 should round UP
		// 105 % 11 = 6
		long startTicks = 105;
		var interval = TimeSpan.FromTicks(11);

		// Act
		DateTime result = dt(startTicks).Round(interval);

		// Assert: delta=6, 6*2=12 >= 11, rounds UP to 110
		Equal(dt(110), result);
	}

	[Fact]
	public void Round_15MinuteInterval()
	{
		// Arrange: Base=12:00. 12:22:30 is midpoint between 12:15 and 12:30 (7.5 min from each)
		long mid = timeTicks(mins: 22, secs: 30); // 12:22:30
		long aboveMid = timeTicks(mins: 22, secs: 31); // 12:22:31
		var interval = TimeSpan.FromMinutes(15);

		// Act
		DateTime result1 = dt(mid).Round(interval);
		DateTime result2 = dt(aboveMid).Round(interval);

		// Assert
		Equal(time(30), result1); // rounds UP to 12:30 (midpoint)
		Equal(time(30), result2); // rounds UP to 12:30
	}

	[Fact]
	public void Round_PreservesDateTimeKind_Utc()
		=> Equal(DateTimeKind.Utc, _roundDT(30, 1, DateTimeKind.Utc).Kind);

	[Fact]
	public void Round_PreservesDateTimeKind_Local()
		=> Equal(DateTimeKind.Local, _roundDT(30, 1, DateTimeKind.Local).Kind);

	[Fact]
	public void Round_PreservesDateTimeKind_Unspecified()
		=> Equal(DateTimeKind.Unspecified, _roundDT(30, 1, DateTimeKind.Unspecified).Kind);

	// --- Round DateTimeOffset Tests ---

	[Fact]
	public void Round_DateTimeOffset_MidpointRoundsUp()
		=> Equal(timeO(1, offset: -5), _roundDTO(30, 1, -5));

	[Fact]
	public void Round_DateTimeOffset_PreservesOffset()
		=> Equal(TimeSpan.FromHours(7), _roundDTO(30, 1, 7).Offset);

	// --- RoundUp Tests ---

	[Fact]
	public void RoundUp_AlreadyOnBoundary_NoChange()
		=> Equal(time(), _roundUpDT(Base, TimeSpan.FromMinutes(5)));

	[Fact]
	public void RoundUp_OneTickAboveBoundary_RoundsUpFull()
		=> Equal(timeHour(1), _roundUpDT(Base + 1, TimeSpan.FromHours(1)));

	[Fact]
	public void RoundUp_RoundsUpToNextInterval()
	{
		// Arrange: Base + 33 min + 45 sec
		long startTicks = timeTicks(mins: 33, secs: 45);
		var interval = TimeSpan.FromMinutes(15);

		// Act
		DateTime result = dt(startTicks).RoundUp(interval);

		// Assert: rounds up to 45-minute mark (12:45)
		Equal(time(45), result);
	}

	[Fact]
	public void RoundUp_PreservesDateTimeKind()
		=> Equal(DateTimeKind.Utc, _roundUpDT(timeTicks(mins: 33), TimeSpan.FromMinutes(15), DateTimeKind.Utc).Kind);

	[Fact]
	public void RoundUp_DateTimeOffset_PreservesOffset()
	{
		// Arrange: Base + 33 min + 45 sec, offset -8
		long startTicks = timeTicks(mins: 33, secs: 45);
		var interval = TimeSpan.FromMinutes(15);

		// Act
		DateTimeOffset result = _roundUpDTO(startTicks, interval, -8);

		// Assert
		Equal(TimeSpan.FromHours(-8), result.Offset);
		Equal(timeO(45, offset: -8), result);
	}

	// --- RoundDown Tests ---

	[Fact]
	public void RoundDown_AlreadyOnBoundary_NoChange()
		=> Equal(time(), _roundDownDT(Base, TimeSpan.FromMinutes(5)));

	[Fact]
	public void RoundDown_OneTickAboveBoundary_RoundsDownToBase()
		=> Equal(time(), _roundDownDT(Base + 1, TimeSpan.FromHours(1)));

	[Fact]
	public void RoundDown_RoundsDownToPreviousInterval()
	{
		// Arrange: Base + 43 min + 59 sec
		long startTicks = timeTicks(mins: 43, secs: 59);
		var interval = TimeSpan.FromMinutes(15);

		// Act
		DateTime result = dt(startTicks).RoundDown(interval);

		// Assert: rounds down to 30-minute mark (12:30)
		Equal(time(30), result);
	}

	[Fact]
	public void RoundDown_PreservesDateTimeKind()
		=> Equal(DateTimeKind.Local, _roundDownDT(timeTicks(mins: 43), TimeSpan.FromMinutes(15), DateTimeKind.Local).Kind);

	[Fact]
	public void RoundDown_DateTimeOffset_PreservesOffset()
	{
		// Arrange: Base + 43 min + 59 sec, offset +3
		long startTicks = timeTicks(mins: 43, secs: 59);
		var interval = TimeSpan.FromMinutes(15);

		// Act
		DateTimeOffset result = _roundDownDTO(startTicks, interval, 3);

		// Assert
		Equal(TimeSpan.FromHours(3), result.Offset);
		Equal(timeO(30, offset: 3), result);
	}

	// --- Edge Cases ---

	[Fact]
	public void Round_VerySmallInterval_OneTick()
	{
		// Arrange
		long startTicks = 100;
		var interval = TimeSpan.FromTicks(1);

		// Act
		DateTime result = dt(startTicks).Round(interval);

		// Assert: should be exactly the same
		Equal(dt(startTicks), result);
	}

	[Fact]
	public void Round_HourInterval_CommonScenario()
	{
		// Arrange: Base at 12:00, shift to 14:00 for test
		long base2pm = timeTicks(hours: 2); // 14:00:00
		long t1 = timeTicks(hours: 2, mins: 29, secs: 59); // 14:29:59
		long t2 = timeTicks(hours: 2, mins: 30);            // 14:30:00 (midpoint)
		long t3 = timeTicks(hours: 2, mins: 30, secs: 1);  // 14:30:01
		var interval = TimeSpan.FromHours(1);

		// Act
		DateTime result1 = dt(t1).Round(interval);
		DateTime result2 = dt(t2).Round(interval);
		DateTime result3 = dt(t3).Round(interval);

		// Assert
		Equal(timeHour(2), result1);   // down to 14:00
		Equal(timeHour(3), result2);   // up to 15:00 (midpoint)
		Equal(timeHour(3), result3);   // up to 15:00
	}
}
