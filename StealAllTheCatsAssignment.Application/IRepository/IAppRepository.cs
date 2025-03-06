using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.IRepository
{
    public interface IAppRepository
    {
        public Task<bool> AddCatWithTags(Cat cat, IEnumerable<Tag> tags);
        public Task<IEnumerable<JsonCatDto>?> InitializeAndDeserialize();
        public Task<byte[]> GetImageFromUrl(string url);
        public Task<Cat?> GetCatById(int id);
        public Task<IEnumerable<Cat>?> GetAllCats(string? tagName,int pageNum, int pageSize);
        public Task<Tag?> GetTagByName(string tagName);
    }
}
