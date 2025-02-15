using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Data;
using StealAllTheCatsAssignment.Domain.Models;
using System.Text.Json;

namespace StealAllTheCatsAssignment.Infrastructure.Repository
{
    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public AppRepository(AppDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Cat?> Get(int id)
        {
            return await _context.Cats.Where(x=>x.Id == id).AsNoTracking().SingleOrDefaultAsync();
        }


        public async Task<Tag?> GetTag(string tagName)
        {
            return await _context.Tags.Where(x => x.Name == tagName).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Cat>?> GetAll()
        {
            return await _context.Cats.Include(x=>x.CatTags).AsNoTracking().ToListAsync();
        }

        public async Task<bool> Add(Cat cat, IEnumerable<Tag> tags)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (await _context.Cats.Where(c => c.CatId == cat.CatId).SingleOrDefaultAsync() is null)
                        await _context.Cats.AddAsync(cat);
                    else
                        return true;

                    foreach (var tag in tags)
                    {
                        if (await _context.Tags.Where(c => c.Name == tag.Name).SingleOrDefaultAsync() is null)
                            await _context.Tags.AddAsync(tag);
                    }
                    await _context.SaveChangesAsync();

                    List<CatTag> catTags = new List<CatTag>();
                    foreach (var tag in tags)
                    {
                        CatTag catTag = new CatTag();
                        catTag.CatId = await _context.Cats.Where(x => x.CatId == cat.CatId).Select(x => x.Id).SingleAsync();
                        catTag.TagId = await _context.Tags.Where(x => x.Name == tag.Name).Select(x => x.Id).SingleAsync();
                        catTags.Add(catTag);
                    }
                    await _context.CatTags.AddRangeAsync(catTags);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public async Task<IEnumerable<JsonCatDto>?> InitializeAndDeserialize()
        {
            var baseAddress = _configuration.GetSection("Settings").GetValue<string>("baseUrl");
            if (baseAddress == null)
                return null;
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration.GetValue<string>("apiKey"));
            var response = await _httpClient.GetAsync(_configuration.GetSection("Settings").GetValue<string>("query"));
            var jsonData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<JsonCatDto>>(jsonData); 
        }

        public async Task<byte[]> GetFileFromUrl(string url)
        {
            var image = await _httpClient.GetAsync(url);
            return await image.Content.ReadAsByteArrayAsync();
        }

        public async Task ClearDb()
        {
            if (_context.Cats.Any() || _context.Tags.Any())
            {
                _context.Cats.Clear();
                _context.Tags.Clear();
                await _context.SaveChangesAsync();
            }
            return;
        }
    }
}
