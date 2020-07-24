using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GranEstacion.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GranEstacion.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly GranEstacionContext _db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, GranEstacionContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Log> Get() => _db.Logs.ToArray();
    }
}