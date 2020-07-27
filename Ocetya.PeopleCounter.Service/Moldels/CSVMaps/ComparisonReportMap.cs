namespace Ocetya.PeopleCounter.Service.Moldels.CSVMaps
{
    using CsvHelper.Configuration;

    public class ComparisonReportMap : ClassMap<ComparisonReport>
    {
        public ComparisonReportMap()
        {
            Map(m => m.Camera).Index(0).Name("Camara");
            Map(m => m.Date).Index(1).Name("Fecha");
            Map(m => m.Entered).Index(2).Name("Entradas");
            Map(m => m.LastEntered).Index(3).Name("Entradas del año anterior");
            Map(m => m.Exited).Index(4).Name("Salidas");
            Map(m => m.LastExited).Index(5).Name("Salidas del año anterior");
        }
    }
}