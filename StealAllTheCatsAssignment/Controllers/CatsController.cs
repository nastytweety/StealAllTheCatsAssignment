using Microsoft.AspNetCore.Mvc;
using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Application.IService;
using StealAllTheCatsAssignment.Application.Mapperly;
using StealAllTheCatsAssignment.Domain.Models;
using StealAllTheCatsAssignment.Filters;

namespace StealAllTheCatsAssignment.API.Controllers
{

    [ApiController]
    [Route("api/cats")]
    public class CatsController : ControllerBase
    {

        private readonly ILogger<CatsController> _logger;
        private readonly IAppService _appService;
        private readonly IMapper _mapper;

        public CatsController(ILogger<CatsController> logger, IAppService appService, IMapper mapper)
        {
            _logger = logger;
            _appService = appService;
            _mapper = mapper;
        }

        /// <summary>
        /// This endpoint is filling the database with 25 new cats with their tags from thecatapi.com
        /// </summary>
        /// <response code="200"> Returns success message</response>
        /// <response code="400"> Returns error message</response>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseDto>> Fetch()
        {
            var response = await _appService.DeserializeAndStoreInDb();
            if (response.Status.Equals("200"))
                return Ok(new ResponseDto { Message = "Cats successfully stored in db" });
            return BadRequest(new ResponseDto { Message = "Cats could not be stored in db" });
        }

        /// <summary>
        /// This endpoint is used to provide a cat by its id attribute
        /// </summary>
        /// <param name="id">the cats id as an integer > 0 </param>
        /// <response code="200"> Returns the cat</response>
        /// <response code="404"> Returns Not found if cat id doesn't exist in database</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Cat>?> Get(int id)
        {
            var cat = await _appService.GetCatById(id);
            if(cat is null)
                return NotFound();
            return Ok(cat);
        }

        /// <summary>
        /// This endpoint is used to provide cats with specific tag (if provided) or all the cats in db. 
        /// The endpoint provides paging support
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200"> Returns a list of cats without the cat image for increased speed</response>
        /// <response code="404"> Returns not found if there is not a cat to provide</response>
        [HttpGet]
        [ValidationActionFilter]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CatDto>?> Get([FromQuery] QueryModel query)
        {
            IEnumerable<Cat>? cats;
            if (query.tag is null)
                cats = await _appService.GetAllCats();
            else
                cats = await _appService.GetCatsByTag(query.tag);

            if (cats is null)
                return NotFound();

            return Ok(_mapper.MapCatsToCatDtos(cats.Skip((query.page - 1) * query.pageSize).Take(query.pageSize)));
        }
    }
}
