namespace PaulTechGuy.NetCore.TimeFilters.Tests;

[TestClass]
public class WeekdayTimeRangeTest
{
	private readonly DayOfWeek validDayOfWeek = DayOfWeek.Sunday;
	private readonly TimeSpan validStartTime = new(1, 2, 3);
	private readonly TimeSpan validEndTime = new(13, 14, 15);
	private readonly string validAsStringTime;
	private readonly string validAsStringDayOfWeek;
	private readonly string validAsStringFullRange;
	private readonly string invalidDayOfWeek = "Mooonday";

	public WeekdayTimeRangeTest()
	{
		this.validAsStringTime = $"{this.validStartTime.Hours:00}:{this.validStartTime.Minutes:00}:{this.validStartTime.Seconds:00}-{this.validEndTime.Hours:00}:{this.validEndTime.Minutes:00}:{this.validEndTime.Seconds:00}";
		this.validAsStringDayOfWeek = this.validDayOfWeek.ToString();
		this.validAsStringFullRange = $"{this.validAsStringDayOfWeek} {this.validAsStringTime}";
	}

	[TestInitialize]
	public void TestInitialize()
	{
	}

	[TestMethod]
	public void DayTimeRange_DefaultCtor_PropertiesNull()
	{
		// arrange
		var range = new WeekdayTimeRange();

		// act

		// assert
		Assert.IsTrue(range.DayOfWeek == null);
		Assert.IsTrue(range.StartTime == null);
		Assert.IsTrue(range.EndTime == null);
		Assert.IsTrue(range.ToString().Length == 0);
	}

	[TestMethod]
	public void DayTimeRange_CtorWithDayOfWeek_IsValid()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validDayOfWeek);

		// act

		// assert
		Assert.IsTrue(range.DayOfWeek != null);
		Assert.IsTrue(range.StartTime == null);
		Assert.IsTrue(range.EndTime == null);
		Assert.IsTrue(range.ToString() == this.validDayOfWeek.ToString());
	}

	[TestMethod]
	public void DayTimeRange_CtorWithTimes_IsValid()
	{
		// arrange
		var range = new WeekdayTimeRange(startTime: this.validStartTime, endTime: this.validEndTime);

		// act

		// assert
		Assert.IsTrue(range.DayOfWeek == null);
		Assert.IsTrue(range.StartTime != null);
		Assert.IsTrue(range.EndTime != null);
		Assert.IsTrue(range.ToString() == this.validAsStringTime);
	}

	[TestMethod]
	public void DayTimeRange_CtorWithDayOfWeekTimes_IsValid()
	{
		// arrange
		var range = new WeekdayTimeRange(dayOfWeek: this.validDayOfWeek, startTime: this.validStartTime, endTime: this.validEndTime);

		// act

		// assert
		Assert.IsTrue(range.DayOfWeek != null);
		Assert.IsTrue(range.StartTime != null);
		Assert.IsTrue(range.EndTime != null);
		Assert.IsTrue(range.ToString() == this.validAsStringFullRange);
	}

	[TestMethod]
	public void DayTimeRange_CtorWithStartTimeGreaterThanEndTime_InvalidOperationException()
	{
		// arrange

		// act

		// assert
		Assert.ThrowsException<InvalidOperationException>(() => new WeekdayTimeRange(startTime: this.validEndTime, endTime: this.validStartTime));
	}

	[TestMethod]
	public void DayTimeRange_CtorWithStartTimeEqualEndTime_InvalidOperationException()
	{
		// arrange

		// act

		// assert
		Assert.ThrowsException<InvalidOperationException>(() => new WeekdayTimeRange(startTime: this.validStartTime, endTime: this.validStartTime));
	}

	[TestMethod]
	public void DayTimeRange_CtorStringWeekday_IsValid()
	{
		// arrange
		_ = new WeekdayTimeRange(DayOfWeek.Sunday.ToString());

		// act

		// assert
	}

	[TestMethod]
	public void DayTimeRange_CtorStringTime_IsValid()
	{
		// arrange
		_ = new WeekdayTimeRange("01:02:03", "13:14:15");

		// act

		// assert
	}

	[TestMethod]
	public void DayTimeRange_CtorStringWeekdayTime_IsValid()
	{
		// arrange
		_ = new WeekdayTimeRange(DayOfWeek.Sunday.ToString(), "01:02:03", "13:14:15");

		// act

		// assert
	}

	[TestMethod]
	public void DayTimeRange_CtorStringWeekday_IsInvalid()
	{
		// arrange

		// act

		// assert
		Assert.ThrowsException<InvalidOperationException>(() => new WeekdayTimeRange(this.invalidDayOfWeek));
	}

	[TestMethod]
	public void DayTimeRange_CtorStringTime_IsInvalid()
	{
		// arrange

		// act

		// assert
		Assert.ThrowsException<InvalidOperationException>(() => new WeekdayTimeRange("60:02:03", "13:14:15"));
	}

	[TestMethod]
	public void DayTimeRange_CtorStringWeekdayTime_IsInvalid()
	{
		// arrange

		// act

		// assert
		Assert.ThrowsException<InvalidOperationException>(() => new WeekdayTimeRange(this.invalidDayOfWeek, "60:02:03", "13:14:15"));
	}

	[TestMethod]
	public void DayTimeRange_TryParseDayOfWeek_IsValid()
	{
		// arrange
		string[] days = Enum.GetNames(typeof(DayOfWeek));

		// act
		bool isInvalid = days.Any(d => !WeekdayTimeRange.TryParse(d, out _));

		// assert
		Assert.IsTrue(!isInvalid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseDayOfWeekLowercase_IsValid()
	{
		// arrange
		string[] days = Enum.GetNames(typeof(DayOfWeek));

		// act
		bool isInvalid = days.Any(d => !WeekdayTimeRange.TryParse(d.ToLower(), out _));

		// assert
		Assert.IsTrue(!isInvalid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseDayOfWeek_IsInvalid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse(this.invalidDayOfWeek, out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseTime_IsValid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse(this.validAsStringTime, out WeekdayTimeRange? range);

		// assert
		Assert.IsTrue(isValid);
		Assert.AreEqual(this.validAsStringTime, range?.ToString(), ignoreCase: false);
	}

	[TestMethod]
	public void DayTimeRange_TryParseTimeWithoutSeconds_IsValid()
	{
		// arrange
		string? time = "01:02-13:14";

		// act
		bool isValid = WeekdayTimeRange.TryParse(time, out WeekdayTimeRange? range);

		// assert
		Assert.IsTrue(isValid);
		Assert.AreEqual(time, $"{range?.StartTime?.Hours:00}:{range?.StartTime?.Minutes:00}-{range?.EndTime?.Hours:00}:{range?.EndTime?.Minutes:00}", ignoreCase: false);
	}

	[TestMethod]
	public void DayTimeRange_TryParseTimeInvalidHours_IsInValid()
	{
		// arrange
		string? time = "01:02:03-60:14:00";

		// act
		bool isValid = WeekdayTimeRange.TryParse(time, out WeekdayTimeRange? _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseTimeInvalidMinutes_IsInValid()
	{
		// arrange
		string? time = "01:02:03-13:60:00";

		// act
		bool isValid = WeekdayTimeRange.TryParse(time, out WeekdayTimeRange? _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseTimeInvalidSeconds_IsInValid()
	{
		// arrange
		string? time = "01:02:03-13:14:60";

		// act
		bool isValid = WeekdayTimeRange.TryParse(time, out WeekdayTimeRange? _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseInvalidStartTime_IsInvalid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse("60:02:03-13:14:15", out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseInvalidEndTime_IsInvalid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse("01:02:03:02-60:14:15", out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseEmptyString_IsInvalid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse("", out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseNullString_IsInvalid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse(null, out WeekdayTimeRange _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseLooseWhitespace_IsValid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse("   Monday   10:03:59  -     11:01:06", out _);

		// assert
		Assert.IsTrue(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseLooseWhitespaceWithNoSeconds_IsValid()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse("   Monday   10:03  -     11:01", out _);

		// assert
		Assert.IsTrue(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseArray1_IsValid()
	{
		// arrange
		string[]? ranges = new string[] { "Monday", };

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out _);

		// assert
		Assert.IsTrue(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseArrayN_IsValid()
	{
		// arrange
		string[]? ranges = new string[] { "Monday", "3:10:00-14:20:00", "Friday 03:10-14:20", "   Friday    03:10  -  14:20   ", }; // different variations

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out _);

		// assert
		Assert.IsTrue(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseEmptyArray_IsValid()
	{
		// arrange
		string[]? ranges = Array.Empty<string>();

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out _);

		// assert
		Assert.IsTrue(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseInvalidArray1_IsValid()
	{
		// arrange
		string[]? ranges = new string[] { this.invalidDayOfWeek, };

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseInvalidArray2_IsValid()
	{
		// arrange
		string[]? ranges = new string[] { this.invalidDayOfWeek, "60:00:00"}; // different variations

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_WithinInvalidWeekdayAndTime_IsFalse ()
	{
		// arrange
		var range = new WeekdayTimeRange(DayOfWeek.Sunday, startTime: this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 3).Date.Add(this.validStartTime)); // this date is a Monday, plus past start time

		// assert
		Assert.IsFalse(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinWeekdayAndInvalidTime_IsFalse ()
	{
		// arrange
		var range = new WeekdayTimeRange(DayOfWeek.Sunday, startTime: this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(-1,0,0)))); // this date is a Sunday, plus past start time, minus 1 hour

		// assert
		Assert.IsFalse(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinWeekdayAndTime_IsTrue()
	{
		// arrange
		var range = new WeekdayTimeRange(DayOfWeek.Sunday, startTime: this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(1,0,0)))); // this date is a Sunday, plus past start time, plus 1 hour

		// assert
		Assert.IsTrue(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinWeekday_IsTrue()
	{
		// arrange
		var range = new WeekdayTimeRange(DayOfWeek.Sunday);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date); // this date is a Sunday

		// assert
		Assert.IsTrue(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinInvalidWeekday_IsFalse()
	{
		// arrange
		var range = new WeekdayTimeRange(DayOfWeek.Sunday);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 3).Date); // this date is a Monday

		// assert
		Assert.IsFalse(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinAtStartTime_IsTrue()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime)); // this date plus start time

		// assert
		Assert.IsTrue(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinBeforeStartTime_IsFalse()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(0, 0, -1)))); // this date plus start time minus 1 second

		// assert
		Assert.IsFalse(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinPastStartTime_IsTrue()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(0, 0, 1)))); // this date plus start time plus 1 second

		// assert
		Assert.IsTrue(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinAtEndTime_IsFalse()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validEndTime)); // this date plus end time

		// assert
		Assert.IsFalse(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinBeforeEndTime_IsTrue()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validEndTime).Add(new TimeSpan(0, 0, -1))); // this date plus end time minus 1 second

		// assert
		Assert.IsTrue(isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinPastEndTime_IsFalse ()
	{
		// arrange
		var range = new WeekdayTimeRange(this.validStartTime, endTime: this.validEndTime);

		// act
		bool isWithin = range.Within(new DateTime(2000, 1, 2).Date.Add(this.validEndTime).Add(new TimeSpan(0, 0, 1))); // this date plus end time plus 1 second

		// assert
		Assert.IsFalse (isWithin);
	}

	[TestMethod]
	public void DayTimeRange_WithinArray1_IsTrue()
	{
		// arrange
		string[]? ranges = new string[] { "Sunday", };

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out var rangeCollection);

		// assert
		Assert.IsTrue(isValid);
		if (rangeCollection != null)
		{
			bool isWithin = rangeCollection.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(0, 0, 1)))); // Sunday plus start date plus 1 second
			Assert.IsTrue(isWithin);
		}
	}

	[TestMethod]
	public void DayTimeRange_WithinArrayN_IsTrue()
	{
		// arrange
		string[]? ranges = new string[] { "Monday", $"{this.validStartTime}-{this.validEndTime}", "Friday 03:10-14:20", "   Friday    03:10  -  14:20   ", }; // 

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out var rangeCollection);

		// assert
		Assert.IsTrue(isValid);
		if (rangeCollection != null)
		{
			bool isWithin = rangeCollection.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(0, 0, 1)))); // start date plus 1 second
			Assert.IsTrue(isWithin);
		}
	}
	[TestMethod]
	public void DayTimeRange_WithinArray1_IsFalse()
	{
		// arrange
		string[]? ranges = new string[] { "Monday", };

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out var rangeCollection);

		// assert
		Assert.IsTrue(isValid);
		if (rangeCollection != null)
		{
			bool isWithin = rangeCollection.Within(new DateTime(2000, 1, 2).Date.Add(this.validStartTime.Add(new TimeSpan(0, 0, 1)))); // Sunday plus start date plus 1 second
			Assert.IsFalse(isWithin);
		}
	}

	[TestMethod]
	public void DayTimeRange_WithinArrayN_IsFalse()
	{
		// arrange
		string[]? ranges = new string[] { $"{this.validStartTime}-{this.validEndTime}", "Friday 03:10-14:20", "   Friday    03:10  -  14:20   ", }; // 

		// act
		bool isValid = WeekdayTimeRange.TryParse(ranges, out var rangeCollection);

		// assert
		Assert.IsTrue(isValid);
		if (rangeCollection != null)
		{
			bool isWithin = rangeCollection.Within(new DateTime(2000, 1, 2).Date.Add(this.validEndTime.Add(new TimeSpan(0, 0, 1)))); // end date plus 1 second
			Assert.IsFalse(isWithin);
		}
	}

	[TestMethod]
	public void DayTimeRange_TryParseDayOfWeekStartTimeAfterEndTime()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse($"Sunday {this.validEndTime}-{this.validStartTime}", out _);

		// assert
		Assert.IsFalse(isValid);
	}

	[TestMethod]
	public void DayTimeRange_TryParseStartTimeAfterEndTime()
	{
		// arrange

		// act
		bool isValid = WeekdayTimeRange.TryParse($"{this.validEndTime}-{this.validStartTime}", out _);

		// assert
		Assert.IsFalse(isValid);
	}
}