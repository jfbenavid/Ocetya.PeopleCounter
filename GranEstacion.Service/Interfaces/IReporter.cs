namespace GranEstacion.Service.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReporter
    {
        Task GetAttachedFile();

        Task SaveData();
    }
}