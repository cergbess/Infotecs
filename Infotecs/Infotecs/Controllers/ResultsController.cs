using Infotecs.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Infotecs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly ResultDAL _resultDAL;
        public ResultsController(DbContextOptions<AppDbContext> db)
        {
            _resultDAL = new ResultDAL(db);
        }
        [HttpGet]
        public IActionResult GetResults(
            string fileName = null,
            DateTime? minDateTime = null,
            DateTime? maxDateTime = null,
            double? minAvgIndicatorValue = null,
            double? maxAvgIndicatorValue = null,
            double? minAvgTimeInSeconds = null,
            double? maxAvgTimeInSeconds = null)
        {
            var results = _resultDAL.GetResults(fileName, minDateTime, maxDateTime, 
                minAvgIndicatorValue, maxAvgIndicatorValue, minAvgTimeInSeconds, maxAvgTimeInSeconds);
            
            return Ok(results);
        }
    }
}

