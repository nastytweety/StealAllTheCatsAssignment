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

        /// <summary>
        /// This endpoint is filling the database with 25 new cats with their tags from thecatapi.com
        /// </summary>
        /// <response code="200"> Returns success message</response>
        /// <response code="400"> Returns failure message with explanation</response>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ResponseDto>> Fetch()
        {
            var response = await _appService.DeserializeAndStoreInDb();
            if (response.Status.Equals("200"))
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// This endpoint is used to provide a cat by its id attribute
        /// </summary>
        /// <param name="id">the cats id as an integer > 0 </param>
        /// <response code="200"> Returns the cat</response>
        /// <response code="404"> Returns Not found if cat id doesn't exist in database</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cat?>> Get(int id)
        {
            var cat = await _appService.GetCatById(id);
            if(cat is null)
                return NotFound();
            return Ok(cat);
        }

        /// <summary>
        /// This endpoint is used to provide cats with specific tag (if provided) or all the cats in db. 
        /// The endpoints provides paging support
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200"> Returns a list of cats without the cat image for increased speed</response>
        /// <response code="404"> Returns not found if there is not a cat to provide</response>
        [HttpGet]
        [ValidationActionFilter]
        public async Task<ActionResult<CatDto>?> Get([FromQuery] QueryModel query)
        {
            IEnumerable<CatDto>? cats;
            if (query.tag is null)
                cats = await _appService.GetAllCats();
            else
                cats = await _appService.GetCatsByTag(query.tag);

            if (cats is null)
                return NotFound();

            return Ok(cats.Skip((query.page - 1) * query.pageSize).Take(query.pageSize));
        }
    }
}
