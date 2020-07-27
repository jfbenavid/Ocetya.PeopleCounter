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
    public class CamerasLogController : ControllerBase
    {
        private readonly GranEstacionContext _db;

        public CamerasLogController(GranEstacionContext db)
        {
            _db = db;
            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        public async Task<IEnumerable<ChartLog>> Get()
        {
            var data = await
                _db.Logs
                .Include(log => log.Camera)
                .Where(log => log.Date >= DateTime.Today)
                .GroupBy(log => new
                {
                    log.Camera.CameraId,
                    CameraName = log.Camera.Name,
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
                .ThenBy(g => g.Key.CameraId)
                .Select(g => new Log
                {
                    CameraId = g.Key.CameraId,
                    Camera = new Camera { CameraId = g.Key.CameraId, Name = g.Key.CameraName },
                    Entered = g.Sum(log => log.Entered),
                    Exited = g.Sum(log => log.Exited),
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, g.Key.Minute, 0)
                })
                .ToArrayAsync();

            return data
                .GroupBy(d => new
                {
                    d.CameraId,
                    d.Camera.Name
                })
                .Select(g => new ChartLog
                {
                    Label = g.Key.Name,
                    Data = data
                        .Where(h => h.CameraId == g.Key.CameraId)
                        .Select(h => new List<object>() { h.Date, h.Entered - h.Exited })
                });
        }
    }
}