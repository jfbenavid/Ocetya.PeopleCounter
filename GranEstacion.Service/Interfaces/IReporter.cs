namespace GranEstacion.Service.Interfaces
{
    using GranEstacion.Service.Models;
    using OpenPop.Pop3;
    using System.Threading.Tasks;

    public interface IReporter
    {
        Task GetAttachedFileAsync();

        Task SaveData();
    }
}