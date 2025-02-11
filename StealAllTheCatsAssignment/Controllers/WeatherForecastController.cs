using Microsoft.AspNetCore.Mvc;

namespace StealAllTheCatsAssignment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatsController : ControllerBase
    {

        private readonly ILogger<CatsController> _logger;

        public CatsController(ILogger<CatsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<WeatherForecast> Fetch()
        {

        }
    }
}
