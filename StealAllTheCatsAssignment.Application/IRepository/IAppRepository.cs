using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.IRepository
{
    public interface IAppRepository
    {
        public Task<bool> AddCatWithTags(Cat cat, IEnumerable<Tag> tags);
        public Task<Tag?> GetTag(string tagName);
        public Task<Cat?> GetCat(int id);
        public Task<IEnumerable<Cat>?> GetAllCats(string? tagName);
        public Task<IEnumerable<JsonCatDto>?> InitializeAndDeserialize();
        public Task<byte[]> GetImageFromUrl(string url);
        public Task ClearDb();
    }
}
