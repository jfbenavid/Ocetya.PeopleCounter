namespace GranEstacion.Service.Interfaces
{
    using MimeKit;
    using System.Threading.Tasks;

    public interface IReporter
    {
        Task GetAttachedFileAsync();

        Task SendMailAsync(MimeMessage message);
    }
}