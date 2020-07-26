namespace GranEstacion.Web.Models
{
    using System;
    using System.Collections.Generic;

    public class ChartLog
    {
        public string Label { get; set; }
        public IList<List<object>> Data { get; set; }
    }
}