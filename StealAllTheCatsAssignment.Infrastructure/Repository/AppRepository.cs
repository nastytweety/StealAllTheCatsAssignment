using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StealAllTheCatsAssignment.Infrastructure.Data;
using StealAllTheCatsAssignment.Domain.Models;
using System.Text.Json;
using Azure;
using StealAllTheCatsAssignment.Application.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StealAllTheCatsAssignment.Infrastructure.Repository
{
    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AppRepository> _logger;
        public AppRepository(AppDbContext context, HttpClient httpClient, IConfiguration configuration, ILogger<AppRepository> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> AddCatWithTags(Cat cat, IEnumerable<Tag> tags)
        {
            if (await _context.Cats.Where(c => c.CatId == cat.CatId).SingleOrDefaultAsync() is not null)
                return true;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Cats.AddAsync(cat);
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
                    _logger.LogError(ex.Message);
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

        public async Task<byte[]> GetImageFromUrl(string url)
        {
            var image = await _httpClient.GetAsync(url);
            return await image.Content.ReadAsByteArrayAsync();
        }

        public async Task<Cat?> GetCatById(int id)
        {
            return await _context.Cats.FindAsync(id);
        }

        public async Task<IEnumerable<Cat>?> GetAllCats(string? tagName, int pageNum, int pageSize)
        {
            if(tagName is null)
                return await _context.Cats.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            else
            {
                var tag = await GetTagByName(tagName);
                if (tag is null)
                    return null;
                return await _context.CatTags.Where(x=>x.TagId == tag.Id).Select(x=>x.Cat).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            }
        }

        public async Task<Tag?> GetTagByName(string tagName)
        {
            return await _context.Tags.Where(x => x.Name == tagName).SingleOrDefaultAsync();
        }

    }
}
