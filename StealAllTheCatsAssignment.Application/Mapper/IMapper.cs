using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.Mapper
{
    public interface IMapper
    {
        public Task<Cat> MapJsonCatDtoToCatEntity(JsonCatDto catDto);
        public IEnumerable<Tag> MapJsonCatDtoToTagEntity(JsonCatDto catDto);
    }
}
