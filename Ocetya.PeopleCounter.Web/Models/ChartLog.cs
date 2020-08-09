namespace Ocetya.PeopleCounter.Web.Models
{
    using System.Collections.Generic;

    public class ChartLog
    {
        public string Label { get; set; }
        public IEnumerable<IList<object>> Data { get; set; }
    }
}