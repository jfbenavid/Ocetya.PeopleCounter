namespace GranEstacion.Service.Moldels
{
    using System;

    public class DailyReport
    {
        public DateTime Date { get; set; }
        public string Camera { get; set; }
        public int Entered { get; set; }
        public int Exited { get; set; }
    }
}