namespace Ocetya.PeopleCounter.Service.Interfaces
{
    using System.Threading.Tasks;
    using MimeKit;

    public interface IReportBuilder
    {
        Task<MimeMessage> BuildDailyReportAsync();

        Task<MimeMessage> BuildDayComparisonReport();
    }
}