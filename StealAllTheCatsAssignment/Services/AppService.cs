using Microsoft.EntityFrameworkCore;
using StealAllTheCatsAssignment.Data;
using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Mapper;
using StealAllTheCatsAssignment.Models;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace StealAllTheCatsAssignment.Services
{
    public class AppService : IAppService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AppService(AppDbContext appDbContext, HttpClient httpClient, IConfiguration configuration,IMapper mapper) {
            _context = appDbContext;
            _httpClient = httpClient;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseDto> DeserializeAndStoreInDb()
        {
            var jsonData = await InitializeClient();
            List<JsonCatDto>? catsWithTags = JsonSerializer.Deserialize<List<JsonCatDto>>(jsonData);
            if(catsWithTags is null)
                return new ResponseDto { Status="400", Message="Could not get any data from thecatapi"};

            foreach (var catwithtags in catsWithTags) 
            {
                var cat = await _mapper.MapJsonCatDtoToCatEntity(catwithtags);
                var tags = _mapper.MapJsonCatDtoToTagEntity(catwithtags);
                await StoreInDb(cat,tags);
            }
            return new ResponseDto { Status="200", Message = "Cats stored in db successfully" };  
        }

        public async Task<CatDto?> GetCatById(int id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat is null)
                return null;
            return _mapper.MapCatToCatDto(cat);
        }

        public async Task<IEnumerable<CatDto?>> GetCatsByTag(string tag)
        {
            var tagId = await _context.Tags.Where(x=>x.Name == tag).Select(x=>x.Id).SingleAsync();
            var cats = await _context.Cats.Include(x => x.CatTags)
                                      .Where(x => x.CatTags.Select(x=>x.TagId).Contains(tagId))
                                      .AsNoTracking().ToListAsync();
            if (cats is null)
                return null;
            else
                return _mapper.MapCatToCatDto(cats);
        }

        public async Task<IEnumerable<CatDto?>> GetAllCats()
        {
            var cats = await _context.Cats.ToListAsync();
            if (cats is null)
                return null;
            else
                return _mapper.MapCatToCatDto(cats);
        }

        private async Task<string> InitializeClient()
        {
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("Settings").GetValue<string>("baseUrl"));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration.GetSection("Settings").GetValue<string>("apiKey"));
            var response = await _httpClient.GetAsync(_configuration.GetSection("Settings").GetValue<string>("query"));
            return await response.Content.ReadAsStringAsync();
        }

        private async Task ClearDb()
        {
            if (_context.Cats.Any() || _context.Tags.Any())
            {
                _context.Cats.Clear();
                _context.Tags.Clear();
                await _context.SaveChangesAsync();
            }
            return;
        }

        private async Task StoreInDb(Cat cat, IEnumerable<Tag> tags)
        {
            if (await _context.Cats.Where(c => c.CatId == cat.CatId).SingleOrDefaultAsync() is null)
                await _context.Cats.AddAsync(cat);
            foreach(var tag in tags)
            {
                if(await _context.Tags.Where(c => c.Name == tag.Name).SingleOrDefaultAsync() is null)
                    await _context.Tags.AddAsync(tag);
            }
            await _context.SaveChangesAsync();

            List<CatTag> catTags = new List<CatTag>();
            foreach (var tag in tags)
            {
                CatTag catTag = new CatTag();
                catTag.CatId = await _context.Cats.Where(x=>x.CatId == cat.CatId).Select(x=>x.Id).SingleAsync();
                catTag.TagId = await _context.Tags.Where(x=>x.Name == tag.Name).Select(x => x.Id).SingleAsync(); 
                catTags.Add(catTag);
            }
            await _context.CatTags.AddRangeAsync(catTags);
            await _context.SaveChangesAsync();
        }
    }
}
