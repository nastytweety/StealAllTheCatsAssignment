using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.Mapper
{
    public class Mapper : IMapper
    {
        private readonly IAppRepository _appRepository;
        public Mapper(IAppRepository appRepository)
        {
              _appRepository = appRepository;
        }
        public async Task<Cat> MapJsonCatDtoToCatEntity(JsonCatDto catDto)
        {
            Cat cat = new Cat();
            cat.CatId = catDto.id;
            cat.Width = catDto.width;
            cat.Height = catDto.height;
            cat.Image = await _appRepository.GetFileFromUrl(catDto.url);
            return cat;
        }

        public IEnumerable<Tag> MapJsonCatDtoToTagEntity(JsonCatDto catDto)
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
