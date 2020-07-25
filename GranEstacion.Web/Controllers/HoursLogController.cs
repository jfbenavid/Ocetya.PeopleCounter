namespace GranEstacion.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GranEstacion.Repository;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class HoursLogController : ControllerBase
    {
        private readonly GranEstacionContext _db;

        public HoursLogController(GranEstacionContext db)
        {
            _db = db;
        }

        [HttpGet("{hours}")]
        public async Task<IEnumerable<Log>> Get(int hours) =>
            await Task.FromResult(
                _db.Logs
                    .Where(log => log.Date >= DateTime.Now.AddHours(-hours))
                    .ToArray());
    }
}