namespace Ocetya.PeopleCounter.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ocetya.PeopleCounter.Repository;
    using Ocetya.PeopleCounter.Web.Models;
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
            var data = await
                _db.Logs
                .Where(log => log.Date > DateTime.Today)
                .GroupBy(log => new { log.Date })
                .OrderBy(g => g.Key.Date)
                .Select(g => new Log
                {
                    Date = g.Key.Date,
                    Exited = g.Sum(log => log.Exited),
                    Entered = g.Sum(log => log.Entered)
                })
                .ToArrayAsync();

            return new List<ChartLog>()
            {
                new ChartLog {
                    Label = "Personas",
                    Data = data
                        .Select(log => new List<object>() { log.Date, log.Entered - log.Exited })
                        .ToList()
                }
            };
        }
    }
}