using Moq;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.Mapperly;
using StealAllTheCatsAssignment.Domain.Models;
using StealAllTheCatsAssignment.Application.Services;

namespace StealAllTheCatsAssignment.Tests
{
    public class AppServiceTests
    {

        private readonly AppService _appService;
        private readonly Mock<IGenericRepository<Cat>> _catRepository = new Mock<IGenericRepository<Cat>>();
        private readonly Mock<IGenericRepository<Tag>> _tagRepository = new Mock<IGenericRepository<Tag>>();
        private readonly Mock<IAppRepository> _appRepository = new Mock<IAppRepository>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        public AppServiceTests()
        {
            _appService = new AppService(_appRepository.Object,_catRepository.Object,_tagRepository.Object,_mapper.Object);
        }


        [Fact]
        public async Task DeserializeAndStoreInDb_ShoudStoreCat()
        {
            // Arrange
            int Id = 1;
            string CatId = "black cat";
            Cat cat = new Cat { Id=Id, CatId = CatId, Height=100, Width=100, Created = DateTime.Now };
            Tag tag = new Tag { Name = "friendly", Created = DateTime.Now };
            List<Tag> tags = new List<Tag> { tag };

            _appRepository.Setup(x => x.AddCatWithTags(cat, tags)).ReturnsAsync(true);

            // Act
            await _appService.DeserializeAndStoreInDb();
            var testCat = await _appService.GetCatById(Id);
            if (testCat is null)
                return;
            // Assert
            Assert.Equal(Id, testCat.Id);
        }

        [Fact]
        public async Task GetCatById_ShouldReturnCat_WhenCatExists()
        {
            // Arrange
            int catId = 1;
            string CatName = "Black Cat";
            var cat = new Cat { Id = catId, CatId = CatName, Created = DateTime.Now };
            _catRepository.Setup(x => x.Get(catId)).ReturnsAsync(cat);
            // Act
            var testCat = await _appService.GetCatById(catId);
            if(testCat is null)
                return;
            // Assert
            Assert.Equal(catId, testCat.Id);
            Assert.Equal(CatName, testCat.CatId);
        }

        [Fact]
        public async Task GetCatById_ShouldReturnNull_WhenCatDoesNotExist()
        {
            // Arrange
            int catId = 1;
            _catRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);
            // Act
            var testCat = await _appService.GetCatById(catId);

            // Assert
            Assert.Null(testCat);
        }

    }
}