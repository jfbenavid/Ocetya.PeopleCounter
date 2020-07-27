namespace GranEstacion.Service.Interfaces
{
    using MimeKit;
    using System.Threading.Tasks;

    public interface IReporter
    {
        Task GetAndSaveNewDataAsync();

        Task SendMailAsync(MimeMessage message);
    }
}