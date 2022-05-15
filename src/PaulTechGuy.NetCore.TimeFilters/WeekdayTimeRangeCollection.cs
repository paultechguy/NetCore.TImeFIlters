namespace PaulTechGuy.NetCore.TimeFilters;

using System;
using System.Linq;

public class WeekdayTimeRangeCollection
{
	private readonly WeekdayTimeRange[] ranges;

	public WeekdayTimeRangeCollection(WeekdayTimeRange[] ranges)
	{
		this.ranges = ranges;
	}

	public bool Within(DateTime time)
	{
		return !this.ranges.All(r => !r.Within(time));
	}
}
