using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StealAllTheCatsAssignment.Data;
using System.Collections.Generic;

namespace StealAllTheCatsAssignment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatsController : ControllerBase
    {

        private readonly ILogger<CatsController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CatsController(ILogger<CatsController> logger, AppDbContext appDbContext, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<string>>> Fetch()
        {
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("Settings").GetValue<string>("baseUrl"));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration.GetSection("Settings").GetValue<string>("apiKey"));
            var response = await _httpClient.GetAsync("?limit=25&has_breeds=1");
            var data = await response.Content.ReadAsStringAsync();
            return Ok(data);
        }
    }
}
