namespace Ocetya.PeopleCounter.Service.Interfaces
{
    using System.Threading.Tasks;
    using MimeKit;

    public interface IReporter
    {
        Task GetAndSaveNewDataAsync();

        Task SendMailAsync(MimeMessage message);
    }
}