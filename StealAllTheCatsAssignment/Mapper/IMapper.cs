using StealAllTheCatsAssignment.DTOs;
using StealAllTheCatsAssignment.Models;

namespace StealAllTheCatsAssignment.Mapper
{
    public interface IMapper
    {
        public Task<Cat> MapJsonCatDtoToCatEntity(JsonCatDto catDto);
        public IEnumerable<Tag> MapJsonCatDtoToTagEntity(JsonCatDto catDto);
        public CatDto MapCatToCatDto(Cat cat);
        public IEnumerable<CatDto> MapCatToCatDto(IEnumerable<Cat> cats);
    }
}
