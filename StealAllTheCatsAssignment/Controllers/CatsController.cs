using Microsoft.AspNetCore.Mvc;
using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Filters;
using StealAllTheCatsAssignment.Models;
using StealAllTheCatsAssignment.Services;

namespace StealAllTheCatsAssignment.Controllers
{

    [ApiController]
    [Route("api/cats")]
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
        public async Task<ActionResult<CatDto?>> Get(int id)
        {
            var cat = await _appService.GetCatById(id);
            if(cat is null)
                return NotFound();
            return Ok(cat);
        }

        [HttpGet]
        [ValidationActionFilter]
        public async Task<ActionResult<CatDto?>> Get([FromQuery] QueryModel query)
        {
            IEnumerable<CatDto?> cats;
            if (query.tag is null)
                cats = await _appService.GetAllCats();
            else
                cats = await _appService.GetCatsByTag(query.tag);

            if (cats is null)
                return NoContent();

            return Ok(cats.Skip((query.page - 1) * query.pageSize).Take(query.pageSize));
        }
    }
}
