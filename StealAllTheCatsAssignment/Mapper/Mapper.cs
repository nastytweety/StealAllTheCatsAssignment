using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Models;
using System.Net.Http;

namespace StealAllTheCatsAssignment.Mapper
{
    public class Mapper : IMapper
    {
        private readonly HttpClient _httpClient;
        public Mapper(HttpClient httpClient)
        {
              _httpClient = httpClient;
        }
        public async Task<Cat> MapJsonCatDtoToCatEntity(JsonCatDto catDto)
        {
            Cat cat = new Cat();
            cat.CatId = catDto.id;
            cat.Width = catDto.width;
            cat.Height = catDto.height;
            cat.Image = await GetFileFromUrl(catDto.url);
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

        public CatDto MapCatToCatDto(Cat cat)
        {
            var catDto = new CatDto();
            catDto.CatId = cat.CatId;
            catDto.Width = cat.Width;
            catDto.Height = cat.Height;
            catDto.Created = cat.Created;
            return catDto;
        }

        public IEnumerable<CatDto> MapCatToCatDto(IEnumerable<Cat> cats)
        {
            List<CatDto> catList = new List<CatDto>(); 
            foreach(var cat in cats)
            {
                var catDto = new CatDto();
                catDto.CatId = cat.CatId;
                catDto.Width = cat.Width;
                catDto.Height = cat.Height;
                catDto.Created = cat.Created;
                catList.Add(catDto);
            }
            return catList;
        }

        private async Task<byte[]> GetFileFromUrl(string url)
        {
            var image = await _httpClient.GetAsync(url);
            return await image.Content.ReadAsByteArrayAsync();
        }
    }
}
