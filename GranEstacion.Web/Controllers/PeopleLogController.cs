namespace GranEstacion.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GranEstacion.Repository;
    using GranEstacion.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class PeopleLogController : ControllerBase
    {
        private readonly GranEstacionContext _db;

        public PeopleLogController(GranEstacionContext db)
        {
            _db = db;
            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        public async Task<IEnumerable<ChartLog>> Get()
        {
            var now = DateTime.Now;
            var referenceDate = new DateTime(now.Year, now.Month, 21);

            var data = await Task.FromResult(
                _db.Logs
                .Where(log => log.Date > referenceDate) //todo: replace referenceDate by DateTime.Now.Date
                .GroupBy(log => new
                {
                    log.Date.Year,
                    log.Date.Month,
                    log.Date.Day,
                    log.Date.Hour,
                    Minute = (log.Date.Minute * 15) / 15
                })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ThenBy(g => g.Key.Day)
                .ThenBy(g => g.Key.Hour)
                .ThenBy(g => g.Key.Minute)
                .Select(g => new Log
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, g.Key.Minute, 0),
                    Exited = g.Sum(log => log.Exited),
                    Entered = g.Sum(log => log.Entered)
                })
                .ToArray());

            return new List<ChartLog>()
            {
                new ChartLog {
                    Label = "Personas",
                    Data = data.Select(log => new List<object>() { log.Date, log.Entered - log.Exited }).ToList()
                }
            };
        }
    }
}