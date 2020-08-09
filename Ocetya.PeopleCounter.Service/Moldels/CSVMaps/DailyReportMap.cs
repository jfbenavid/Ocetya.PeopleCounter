namespace Ocetya.PeopleCounter.Service.Moldels.CSVMaps
{
    using CsvHelper.Configuration;

    public class DailyReportMap : ClassMap<DailyReport>
    {
        public DailyReportMap()
        {
            Map(m => m.Camera).Index(0).Name("Camara");
            Map(m => m.Date).Index(1).Name("Fecha");
            Map(m => m.Entered).Index(2).Name("Entradas");
            Map(m => m.Exited).Index(3).Name("Salidas");
        }
    }
}