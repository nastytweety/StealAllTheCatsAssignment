using Riok.Mapperly.Abstractions;
using StealAllTheCatsAssignment.Application.DTOs;
using StealAllTheCatsAssignment.Domain.Models;

namespace StealAllTheCatsAssignment.Application.Mapperly
{
    [Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
    public partial class Mapper : IMapper
    {
        [MapperIgnoreSource(nameof(Cat.Id))]
        [MapperIgnoreSource(nameof(Cat.Image))]
        [MapperIgnoreSource(nameof(Cat.CatTags))]
        public partial CatDto MapCatToCatDto(Cat cat);
        public partial IEnumerable<CatDto> MapCatsToCatDtos(IEnumerable<Cat> cats);


        [MapProperty(nameof(JsonCatDto.id), nameof(Cat.CatId))]
        [MapperIgnoreSource(nameof(JsonCatDto.breeds))]
        [MapperIgnoreSource(nameof(JsonCatDto.url))]
        [MapperIgnoreTarget(nameof(Cat.Id))]
        [MapperIgnoreTarget(nameof(Cat.CatTags))]
        [MapperIgnoreTarget(nameof(Cat.Created))]
        [MapperIgnoreTarget(nameof(Cat.Image))]
        public partial Cat MapJsonCatDtoToCatEntity(JsonCatDto catDto);
    }
}
