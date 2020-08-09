namespace Ocetya.PeopleCounter.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Ocetya.PeopleCounter.Repository;

    [Route("api/[controller]")]
    [ApiController]
    public class CompareController : ControllerBase
    {
        private readonly GranEstacionContext db;

        public CompareController(GranEstacionContext db)
        {
            this.db = db;
        }
    }
}