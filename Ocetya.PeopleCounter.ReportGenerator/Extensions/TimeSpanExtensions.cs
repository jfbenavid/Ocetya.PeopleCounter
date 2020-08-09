namespace Ocetya.PeopleCounter.ReportGenerator.Extensions
{
    using System;

    public static class TimeSpanExtensions
    {
        public static TimeSpan NearestMinutes(this TimeSpan input, int minutes = 15)
        {
            var totalMinutes = (int)(input - new TimeSpan(0, minutes / 2, 0)).TotalMinutes;

            return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
        }

        public static TimeSpan NextTimeSpan(this TimeSpan input)
        {
            var now = DateTime.Now;

            var nextMinute = new DateTime(((now.Ticks + input.Ticks - 1) / input.Ticks * input.Ticks));
            var next = nextMinute.AddSeconds(1);

            return next - now;
        }
    }
}