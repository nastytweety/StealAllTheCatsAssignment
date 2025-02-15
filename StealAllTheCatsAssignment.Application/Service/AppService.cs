using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.IService;
using StealAllTheCatsAssignment.Domain.Models;
using StealAllTheCatsAssignment.Application.Mapperly;

namespace StealAllTheCatsAssignment.Services
{
    public class AppService : IAppService
    {
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;

        public AppService(IAppRepository appRepository,IMapper mapper) {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> DeserializeAndStoreInDb()
        {
            var catsWithTags = await _appRepository.InitializeAndDeserialize();
            if (catsWithTags is null) 
                return new ResponseDto { Status ="404", Message="Cats not found in thecatapi.com"};

            foreach (var catWithTags in catsWithTags) 
            {
                var cat = _mapper.MapJsonCatDtoToCatEntity(catWithTags);
                cat.Image = await _appRepository.GetFileFromUrl(catWithTags.url);
                var tags = MapJsonCatDtoToTagEntity(catWithTags);
                var response = await _appRepository.Add(cat, tags);
                if(response == false)
                    return new ResponseDto { Status = "400", Message = "Cats could not be stored" };
            }
            return new ResponseDto { Status = "200", Message = "Success" };
        }

        public async Task<Cat?> GetCatById(int id)
        {
            return await _appRepository.Get(id);
        }

        public async Task<IEnumerable<Cat>?> GetCatsByTag(string tagName)
        {
            var cats = await _appRepository.GetAll();
            var tag = await _appRepository.GetTag(tagName);
            if (cats is null || tag is null)
                return null;
            var catsWithTag = cats.Where(x => x.CatTags.Select(x=>x.TagId).Contains(tag.Id)).ToList();
            return catsWithTag;
        }

        public async Task<IEnumerable<Cat>?> GetAllCats()
        {
            return await _appRepository.GetAll();
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
