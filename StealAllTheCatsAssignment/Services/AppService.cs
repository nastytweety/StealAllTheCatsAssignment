using StealAllTheCatsAssignment.Data;
using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Models;
using System.Text.Json;

namespace StealAllTheCatsAssignment.Services
{
    public class AppService : IAppService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AppService(AppDbContext appDbContext, HttpClient httpClient, IConfiguration configuration) {
            _context = appDbContext;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ResponseDto> DeserializeAndStoreInDb()
        {
            var jsonData = await InitializeClient();
            List<CatDto>? catsWithTags = JsonSerializer.Deserialize<List<CatDto>>(jsonData);
            if(catsWithTags is null)
                return new ResponseDto { Status="400", Message="Could not get any data from thecatapi"};
            foreach (var catwithtags in catsWithTags) 
            {
                var cat = await MapCatDtoToCatEntity(catwithtags);
                var tags = await MapCatDtoToTagEntity(catwithtags);
                cat.Tags = tags;
                _context.Cats.Add(cat);
            }
            await _context.SaveChangesAsync();
            return new ResponseDto { Status="200", Message = "Cats stored in db successfully" };  
        }

        public async Task<Cat?> GetCatById(int id)
        {
            return await _context.Cats.FindAsync(id);
        }


        private async Task<Cat> MapCatDtoToCatEntity(CatDto catDto)
        {
            Cat cat = new Cat();
            cat.CatId = catDto.id;
            cat.Width = catDto.width;
            cat.Height = catDto.height;
            cat.Image = await GetFileFromUrl(catDto.url);
            return cat;
        }

        private async Task<List<Tag>> MapCatDtoToTagEntity(CatDto catDto)
        {
            List<Tag> tags = new List<Tag>();
            foreach (var breed in catDto.breeds)
            {
                var temperaments = breed.temperament.ToString().Split(',').ToList();
                foreach (var temperament in temperaments)
                {
                    Tag tag = new Tag();
                    tag.Name = temperament;
                    tags.Add(tag);
                }
            }
            return tags;
        }

        private async Task<byte[]> GetFileFromUrl(string url)
        {
            var image = await _httpClient.GetAsync(url);
            return await image.Content.ReadAsByteArrayAsync();
        }

        private async Task<string> InitializeClient()
        {
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("Settings").GetValue<string>("baseUrl"));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration.GetSection("Settings").GetValue<string>("apiKey"));
            var response = await _httpClient.GetAsync("?limit=25&has_breeds=1");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
