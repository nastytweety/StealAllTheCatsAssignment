using Microsoft.AspNetCore.Mvc;
using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Models;
using StealAllTheCatsAssignment.Services;
using System.Net;

namespace StealAllTheCatsAssignment.Controllers
{
    
    [ApiController]
    [Route("api/cats/")]
    public class CatsController : ControllerBase
    {

        private readonly ILogger<CatsController> _logger;
        private readonly IAppService _appService;

        public CatsController(ILogger<CatsController> logger, IAppService appService)
        {
            _logger = logger;
            _appService = appService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ResponseDto>> Fetch()
        {
            var response = await _appService.DeserializeAndStoreInDb();
            if (response.Status.Equals("200"))
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cat?>> Get(int id)
        {
            var cat = await _appService.GetCatById(id);
            if(cat is null)
                return NotFound();
            return Ok(cat);
        }

        [HttpGet]
        public async Task<ActionResult<Cat?>> Get([FromQuery]string? tag,int page=1,int pageSize=10)
        {
            IEnumerable<Cat?> cats;
            if (tag is null)
                cats = await _appService.GetAllCats();
            else
                cats = await _appService.GetCatsByTag(tag);

            if (cats is null)
                return NoContent();

            return Ok(cats.Skip((page - 1) * pageSize).Take(pageSize));
        }
    }
}
