namespace Ocetya.PeopleCounter.ReportGenerator.Extensions
{
    using System;

    public static class TimeSpanExtensions
    {
        public static TimeSpan NextTimeSpan(this TimeSpan input)
        {
            var now = DateTime.Now;

            var nextMinute = new DateTime((now.Ticks + input.Ticks - 1) / input.Ticks * input.Ticks);
            var next = nextMinute.AddSeconds(1);

            return next - now;
        }
    }
}