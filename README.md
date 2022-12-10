# PaulTechGuy.NetCore.TimeFilters

GitHub: [https://github.com/paultechguy/NetCore.TimeFilters](https://github.com/paultechguy/NetCore.TimeFilters)

## Requirements
- Windows 10 or higher
- Visual Studio 2022 with .NET Core

## WeekdayTimeRange
This .NET Core library provides the `WeekdayTimeRange` which provides support for filtering a `DateTime` object against one more *day of week* patterns.  A *day of week* pattern contains a weekday or start/end time, or both.  For example, if you want to create a *day of week* filter for Sunday, you could use the following source code:

    WeekdayTimeRange range = new(DayOfWeek.Sunday);

There are several more ways to create a `WeekdayTimeRange`:

    WeekdayTimeRange range = new(DayOfWeek.Sunday, new TimeSpan(1, 2, 3), new TimeSpan(13, 14, 15)); // Sunday 01:02:03 to 13:14:15
    WeekdayTimeRange range = new("Sunday", "01:02:03", "13:14:15");

    WeekdayTimeRange range = new(DayOfWeek.Sunday);
    WeekdayTimeRange range = new("Sunday");
    WeekdayTimeRange range = new("Sun"); // using 3-character abbreviations

    WeekdayTimeRange range = new(new TimeSpan(1, 2, 3), new TimeSpan(13, 14, 15)); // 01:02:03 to 13:14:15
    WeekdayTimeRange range = new("01:02:03", "13:14:15");

## TryParse
You can use several `TryParse` method overides to verify if a text string is a valid `WeekdayTimeRange`:

    bool isValid = WeekdayTimeRange.TryParse("Sunday", out var range) ...
    bool isValid = WeekdayTimeRange.TryParse("Sun 01:02:03-13:14:15", out var range) ...
    bool isValid = WeekdayTimeRange.TryParse("01:02:03-13:14:15", out var range) ...

## Within
Once you have a filter, you can determine if a `DateTime` object is *within* the filter range:

    if (range.Within(DateTime.Now)) ...

You can also determine if a collection of `DateTime` objects is *within* a filter range, and have the
`DateTime` matches returned as part of the same method call:

    // determine if a Friday WeekdayTimeRange matches a
    // collection of DateTime objects
    var range = new WeekdayTimeRange(DayOfWeek.Friday);
    var dates = new List<DateTime>()
    {
        // Friday matche(s)
        DateTime.Parse("12/9/2022"),
        DateTime.Parse("1/7/2022"),
        DateTime.Parse("8/11/1911"),

        // miss(es)
        DateTime.Parse("11/13/1915"), // saturday
        DateTime.Parse("7/8/1649"), // sunday
    };

    // returns true with matches count = 3
    bool isValid = range.Within(dates, out var matches);

## WeekdayTimeRangeCollection
You can determine if a `DateTime` object is within a collection of *day of week* filters using:

    string[] patterns = new string[] {"Sunday","Monday 01:02:03-13:14:15", "23:00:00-23:15:00"};
    if (WeekayTimeRange.TryParse(patterns, out WeekdayTimeRangeCollection rangeCollection))
    {
        if (rangeCollection.Within(DateTime.Now)) ...
    }

## .editorConfig
The code has its own set of formatting and language rules.  If you don't like these, feel free
to modify the .editorConfig file, or remove it entirely from the project.

## License
[MIT](https://github.com/paultechguy/WinService.NetCore/blob/develop/LICENSE.txt)

## Inspiration
This class library was a valuable resource for a Windows Service that periodically monitors the computer CPU level. The service needs to bypass the CPU check during certain weekday and/or times of the day.

## Version History
* 1.0.0 - Initial version
* 1.0.1
   + Allow 3-character abbreviations for weekdays
   + Change Within() method to virtual to allow derived classes to override
   + Update unit test nuget packages
   + Update unit tests
* 1.x.x (no nuget packages yet)
   + Add additional Within() to match on a collection of DateTime and return matches
   + Add additional unit tests for new Within() method