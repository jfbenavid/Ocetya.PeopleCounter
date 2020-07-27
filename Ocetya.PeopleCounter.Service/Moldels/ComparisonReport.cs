namespace Ocetya.PeopleCounter.Service.Moldels
{
    public class ComparisonReport : DailyReport
    {
        public int LastEntered { get; set; }
        public int LastExited { get; set; }
    }
}