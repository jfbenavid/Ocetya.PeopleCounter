namespace Ocetya.PeopleCounter.Service.Interfaces
{
    using MimeKit;
    using System.Threading.Tasks;

    public interface IReportBuilder
    {
        Task<MimeMessage> BuildDailyReportAsync();

        Task<MimeMessage> BuildDayComparisonReport();
    }
}