using Microsoft.Extensions.Logging;
using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.IService;
using StealAllTheCatsAssignment.Application.Mapperly;
using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.Services
{
    public class AppService : IAppService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;

        public AppService(ILogger<AppService> logger,IAppRepository appRepository, IMapper mapper) {
            _logger = logger;
            _appRepository = appRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> DeserializeAndStoreInDb()
        {
            var catsWithTags = await _appRepository.InitializeAndDeserialize();
            if (catsWithTags is null) 
                return new ResponseDto { Status ="400", Message="Cats could not be found in thecatapi.com"};

            foreach (var catWithTags in catsWithTags) 
            {
                var cat = _mapper.MapJsonCatDtoToCatEntity(catWithTags);
                cat.Image = await _appRepository.GetImageFromUrl(catWithTags.url);
                var tags = MapJsonCatDtoToTagEntity(catWithTags);
                var response = await _appRepository.AddCatWithTags(cat, tags);
                if(response == false)
                    return new ResponseDto { Status = "400", Message = "Cats could not be stored in db" };
            }
            _logger.LogWarning("Cats Fetched");
            return new ResponseDto { Status = "200", Message = "Cats successfully stored in db" };
        }

        public async Task<Cat?> GetCatById(int id)
        {
            return await _appRepository.GetCatById(id);
        }

        public async Task<IEnumerable<Cat>?> GetCatsByTag(string? tagName)
        {
            if (tagName is null)
                return await _appRepository.GetAllCats();
            else
            {
                var cats = await _appRepository.GetAllCats();
                var tag = _appRepository.GetTagByName(tagName);
                if (tag is null)
                    return null;
                return cats.Where(x => x.CatTags.Select(x => x.TagId).Contains(tag.Id)).ToList();
            }
        }


        private IEnumerable<Tag> MapJsonCatDtoToTagEntity(JsonCatDto catDto)
        {
            List<Tag> tagsList = new List<Tag>();
            foreach (var breed in catDto.breeds)
            {
                var temperaments = breed.temperament.ToString().Split(',').ToList();
                foreach (var temperament in temperaments)
                {
                    Tag tag = new Tag();
                    tag.Name = temperament.Trim();
                    tagsList.Add(tag);
                }
            }
            return tagsList;
        }

    }
}
