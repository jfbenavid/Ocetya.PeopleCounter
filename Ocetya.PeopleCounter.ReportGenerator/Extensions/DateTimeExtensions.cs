namespace Ocetya.PeopleCounter.ReportGenerator.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime RoundDown(this DateTime input, TimeSpan timeSpan)
        {
            var delta = input.Ticks % timeSpan.Ticks;
            return new DateTime(input.Ticks - delta, input.Kind);
        }
    }
}