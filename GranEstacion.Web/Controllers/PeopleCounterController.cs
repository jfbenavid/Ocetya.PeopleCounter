namespace GranEstacion.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GranEstacion.Repository;
    using GranEstacion.Web.Models;
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
            return await
                _db.Logs
                .Where(log => log.Date > DateTime.Today)
                .GroupBy(log => 1)
                .Select(g => new CountLog
                {
                    TotalPeople = g.Sum(log => log.Entered) - g.Sum(log => log.Exited),
                    Gone = g.Sum(log => log.Exited),
                    Entered = g.Sum(log => log.Entered)
                })
                .FirstOrDefaultAsync();
        }
    }
}