using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Models;

namespace StealAllTheCatsAssignment.Services
{
    public interface IAppService
    {
        public Task<ResponseDto> DeserializeAndStoreInDb();
        public Task<CatDto?> GetCatById(int id);
        public Task<IEnumerable<CatDto?>> GetCatsByTag(string tag);
        public Task<IEnumerable<CatDto?>> GetAllCats();
    }
}
