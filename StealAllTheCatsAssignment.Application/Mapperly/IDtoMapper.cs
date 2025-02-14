
using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.Mapperly
{
    public interface IDtoMapper
    {
        public CatDto MapCatToCatDto(Cat cat);
        public IEnumerable<CatDto> MapCatsToCatDtos(IEnumerable<Cat> cats);
    }
}
