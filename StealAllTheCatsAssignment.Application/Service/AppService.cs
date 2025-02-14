using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.IService;
using StealAllTheCatsAssignment.Domain.Models;
using StealAllTheCatsAssignment.Mapper;

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
            var catsWithTags = await _appRepository.InitializeClient();
            if (catsWithTags == null) 
                return new ResponseDto { Status ="404", Message="Cats not found in thecatapi.com"};

            foreach (var catwithtags in catsWithTags) 
            {
                var cat = await _mapper.MapJsonCatDtoToCatEntity(catwithtags);
                var tags = _mapper.MapJsonCatDtoToTagEntity(catwithtags);
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

    }
}
