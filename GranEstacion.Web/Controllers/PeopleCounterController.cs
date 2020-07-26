namespace GranEstacion.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GranEstacion.Repository;
    using GranEstacion.Web.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class PeopleCounterController : ControllerBase
    {
        private readonly GranEstacionContext _db;

        public PeopleCounterController(GranEstacionContext db)
        {
            _db = db;
            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        public async Task<CountLog> Get()
        {
            var now = DateTime.Now;
            var referenceDate = new DateTime(now.Year, now.Month, 21);

            return await Task.FromResult(
                _db.Logs
                .Where(log => log.Date > referenceDate) //todo: replace referenceDate by DateTime.Now.Date
                .GroupBy(log => 1)
                .Select(g => new CountLog
                {
                    TotalPeople = g.Sum(log => log.Entered) - g.Sum(log => log.Exited),
                    Gone = g.Sum(log => log.Exited),
                    Entered = g.Sum(log => log.Entered)
                })
                .FirstOrDefault());
        }
    }
}