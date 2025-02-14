using Riok.Mapperly.Abstractions;
using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.Mapperly
{
    [Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
    public partial class DtoMapper : IDtoMapper
    {
        [MapperIgnoreSource(nameof(Cat.Id))]
        [MapperIgnoreSource(nameof(Cat.Image))]
        [MapperIgnoreSource(nameof(Cat.CatTags))]
        public partial CatDto MapCatToCatDto(Cat cat);
        public partial IEnumerable<CatDto> MapCatsToCatDtos(IEnumerable<Cat> cats);
    }
}
