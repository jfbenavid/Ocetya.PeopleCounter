namespace GranEstacion.Service.Interfaces
{
    using System.Threading.Tasks;

    public interface IReporter
    {
        Task GetAttachedFileAsync();
    }
}