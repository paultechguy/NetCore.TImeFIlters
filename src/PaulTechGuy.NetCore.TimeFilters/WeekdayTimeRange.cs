namespace PaulTechGuy.NetCore.TimeFilters;

using System.Text.RegularExpressions;

public class WeekdayTimeRange
{
	private const string DayOfWeekPattern = @"Monday|Mon|Tuesday|Tue|Wednesday|Wed|Thursday|Thu|Friday|Fri|Saturday|Sat|Sunday|Sun";
	private const string Time24Pattern = @"([01]?[0-9]|2[0-3]):[0-5][0-9](:[0-5][0-9])?";
	private const string DayTimeRangePattern = @$"^\s*(?<Weekday>{DayOfWeekPattern})?\s*((?<Time1>{Time24Pattern})\s*-\s*(?<Time2>{Time24Pattern}))?\s*$";
	private static readonly Regex RegexDayTimeRange = new(DayTimeRangePattern, RegexOptions.IgnoreCase);

	public DayOfWeek? DayOfWeek { get; private set; } = null;

	public TimeSpan? StartTime { get; private set; } = null;

	public TimeSpan? EndTime { get; private set; } = null;

	public WeekdayTimeRange()
	{
		this.DayOfWeek = null;
		this.StartTime = null;
		this.EndTime = null;
	}

	public WeekdayTimeRange(DayOfWeek dayOfWeek)
		: this(dayOfWeek, null, null)
	{
	}

	public WeekdayTimeRange(TimeSpan startTime, TimeSpan endTime)
		: this(null, startTime, endTime)
	{
	}

	public WeekdayTimeRange(
		DayOfWeek dayOfWeek,
		TimeSpan startTime,
		TimeSpan endTime)
		: this((DayOfWeek?)dayOfWeek, startTime, endTime)
	{
	}

	public WeekdayTimeRange(
		string dayOfWeek,
		string startTime,
		string endTime)
	{
		string textRange = $"{dayOfWeek} {startTime}-{endTime}";
		if (!WeekdayTimeRange.TryParse(textRange, out WeekdayTimeRange? range))
		{
			throw new InvalidOperationException($"{nameof(TryParse)} failed to create a {nameof(WeekdayTimeRange)} using {textRange}");
		}

		this.CopyWeekdayTimeRange(range);
	}

	public WeekdayTimeRange(
		string dayOfWeek)
	{
		if (!WeekdayTimeRange.TryParse(dayOfWeek, out WeekdayTimeRange? range))
		{
			throw new InvalidOperationException($"{nameof(TryParse)} failed to create a {nameof(WeekdayTimeRange)} using {dayOfWeek}");
		}

		this.CopyWeekdayTimeRange(range);
	}

	public WeekdayTimeRange(
		string startTime,
		string endTime)
	{
		string textRange = $"{startTime}-{endTime}";
		if (!WeekdayTimeRange.TryParse(textRange, out WeekdayTimeRange? range))
		{
			throw new InvalidOperationException($"{nameof(TryParse)} failed to create a {nameof(WeekdayTimeRange)} using {textRange}");
		}

		this.CopyWeekdayTimeRange(range);
	}

	private WeekdayTimeRange(
		DayOfWeek? dayOfWeek,
		TimeSpan? startTime,
		TimeSpan? endTime)
	{
		if ((startTime == null && endTime != null) || (startTime != null && endTime == null))
		{
			throw new InvalidOperationException($"{nameof(startTime)} and {nameof(endTime)} must both be null or non-null");
		}
		else if (startTime != null && startTime >= endTime)
		{
			throw new InvalidOperationException($"{nameof(startTime)} cannot be greater than {nameof(endTime)}");
		}

		this.DayOfWeek = dayOfWeek;
		this.StartTime = startTime;
		this.EndTime = endTime;
	}

	private void CopyWeekdayTimeRange(WeekdayTimeRange? range)
	{
		if (range == null)
		{
			throw new InvalidOperationException($"{nameof(range)} cannot be null");
		}
		else
		{
			this.DayOfWeek = range.DayOfWeek;
			this.StartTime = range.StartTime;
			this.EndTime = range.EndTime;
		}
	}

	public override string ToString()
	{
		string? result = null;
		if (this.DayOfWeek != null)
		{
			result = this.DayOfWeek.ToString();
		}

		if (this.StartTime != null && this.EndTime != null)
		{
			if (result != null)
			{
				result += " ";
			}

			// if result is null, string interpollation will provide a blank value
			result = $"{result}{this.StartTime.Value.Hours:00}:{this.StartTime.Value.Minutes:00}:{this.StartTime.Value.Seconds:00}-{this.EndTime.Value.Hours:00}:{this.EndTime.Value.Minutes:00}:{this.EndTime.Value.Seconds:00}";
		}

		return result ?? string.Empty;
	}

	public static bool TryParse(string[] values, out WeekdayTimeRangeCollection? ranges)
	{
		List<WeekdayTimeRange>? returnRanges;
		do
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			returnRanges = new List<WeekdayTimeRange>();

			if (values.Length == 0)
			{
				// empty values array passed in is fine...consider parse successful
				break;
			}

			foreach (string? value in values)
			{
				if (WeekdayTimeRange.TryParse(value, out WeekdayTimeRange? range))
				{
					returnRanges.Add(range ?? throw new NullReferenceException(nameof(range)));
				}
				else
				{
					returnRanges.Clear();
					returnRanges = null;
					break;
				}
			}
			break;

		} while (false);

		ranges = null;
		if (returnRanges != null)
		{
			ranges = new WeekdayTimeRangeCollection(returnRanges.ToArray());
		}

		return returnRanges != null;
	}

	public static bool TryParse(string? value, out WeekdayTimeRange? range)
	{
		range = null;
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}

		Match match = RegexDayTimeRange.Match(value);
		if (!match.Success)
		{
			return false;
		}

		// get group matches
		string groupWeekday = match.Groups["Weekday"].Value;
		string groupTime1 = match.Groups["Time1"].Value;
		string groupTime2 = match.Groups["Time2"].Value;
		DayOfWeek? dayOfWeek = ParseWeekDay(groupWeekday);
		TimeSpan? timeSpan1 = ParseTimeSpan(groupTime1);
		TimeSpan? timeSpan2 = ParseTimeSpan(groupTime2);

		// start sanity checks
		if (!dayOfWeek.HasValue && !timeSpan1.HasValue && !timeSpan2.HasValue)
		{
			return false;
		}

		if ((timeSpan1.HasValue && !timeSpan2.HasValue) || (!timeSpan1.HasValue && timeSpan2.HasValue))
		{
			return false;
		}

		if (timeSpan1.HasValue && timeSpan2.HasValue && timeSpan1 >= timeSpan2)
		{
			return false;
		}

		range = new WeekdayTimeRange
		{
			DayOfWeek = dayOfWeek,
			StartTime = timeSpan1,
			EndTime = timeSpan2,
		};

		return true;
	}

	public virtual bool Within(IEnumerable<DateTime> times, out IEnumerable<DateTime> matches)
	{
		// we don't worry about duplicate dates...match all if they exist
		var foundMatches = new List<DateTime>();
		foreach (DateTime dt in times)
		{
			if (this.Within(dt))
			{
				foundMatches.Add(dt);
			}
		}

		matches = foundMatches;

		return matches.Any();
	}

	public virtual bool Within(DateTime time)
	{
		bool found = true;

		do
		{
			if (this.DayOfWeek.HasValue && this.StartTime.HasValue && this.EndTime.HasValue)
			{
				found = this.DayOfWeek == time.DayOfWeek && time.TimeOfDay >= this.StartTime && time.TimeOfDay < this.EndTime;
				if (found)
				{
					break;
				}
			}

			//
			// we have either;
			//    day of week range
			//    begin and end range
			//

			// day of week segment
			if (this.DayOfWeek.HasValue && !this.StartTime.HasValue && !this.EndTime.HasValue)
			{
				found = this.DayOfWeek == time.DayOfWeek;
				if (found)
				{
					break;
				}
			}

			// begin and end segment
			if (!this.DayOfWeek.HasValue && this.StartTime.HasValue && this.EndTime.HasValue)
			{
				found = time.TimeOfDay >= this.StartTime && time.TimeOfDay < this.EndTime;
				if (found)
				{
					break;
				}
			}

		} while (false);

		return found;
	}

	private static DayOfWeek? ParseWeekDay(string weekday)
	{
		if (string.IsNullOrWhiteSpace(weekday))
		{
			return null;
		}

		// expand any 3-character abbreviations
		if (weekday.Length == 3)
		{
			// if any full weekday names start with the 3-character abbr, use it
			string? abbrMatch = DayOfWeekPattern.Split('|')
				.FirstOrDefault(dow => dow.StartsWith(weekday, StringComparison.OrdinalIgnoreCase));
			if (abbrMatch != null)
			{
				weekday = abbrMatch;
			}
		}

		bool isValidWeekday = Enum.TryParse(weekday, ignoreCase: true, out DayOfWeek dayOfWeek);
		if (!isValidWeekday)
		{
			// the initial regex should have validated this so we'll throw if invalid
			throw new ArgumentOutOfRangeException(nameof(weekday));
		}

		return dayOfWeek;
	}

	private static TimeSpan? ParseTimeSpan(string time)
	{
		if (string.IsNullOrWhiteSpace(time))
		{
			return null;
		}

		// we'll let TimeSpan help us
		bool isValidTime = TimeSpan.TryParse(time, out TimeSpan newTime);
		if (!isValidTime)
		{
			// the initial regex should have validated this so we'll throw if invalid
			throw new ArgumentOutOfRangeException($"Invalid time: {time}");
		}

		return newTime;
	}
}