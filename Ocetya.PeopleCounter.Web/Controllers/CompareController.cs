namespace Ocetya.PeopleCounter.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ocetya.PeopleCounter.Repository;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CompareController : ControllerBase
    {
        private readonly GranEstacionContext _db;

        public CompareController(GranEstacionContext db)
        {
            _db = db;
        }
    }
}