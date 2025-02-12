using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Models;

namespace StealAllTheCatsAssignment.Services
{
    public interface IAppService
    {
        public Task<ResponseDto> DeserializeAndStoreInDb();
        public Task<Cat?> GetCatById(int id);
        public Task<IEnumerable<Cat?>> GetCatsByTag(string tag);
        public Task<IEnumerable<Cat?>> GetAllCats();
    }
}
