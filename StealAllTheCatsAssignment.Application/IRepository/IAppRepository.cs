using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.IRepository
{
    public interface IAppRepository
    {
        public Task<bool> Add(Cat cat, IEnumerable<Tag> tags);
        public Task<Tag?> GetTag(string tagName);
        public Task<Cat?> Get(int id);
        public Task<IEnumerable<Cat>?> GetAll();
        public Task<IEnumerable<JsonCatDto>?> InitializeClient();
        public Task<byte[]> GetFileFromUrl(string url);
        public Task ClearDb();
    }
}
