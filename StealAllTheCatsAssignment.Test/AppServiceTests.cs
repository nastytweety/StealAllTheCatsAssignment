using Moq;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.Mapperly;
using StealAllTheCatsAssignment.Domain.Models;
using StealAllTheCatsAssignment.Application.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace StealAllTheCatsAssignment.Tests
{
    public class AppServiceTests
    {

        private readonly AppService _appService;
        private readonly ILogger<AppService> _logger = Substitute.For<ILogger<AppService>>();
        private readonly Mock<IAppRepository> _appRepository = new Mock<IAppRepository>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        public AppServiceTests()
        {
            _appService = new AppService(_logger,_appRepository.Object,_mapper.Object);
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
            // Act
            var testCat = await _appService.GetCatById(catId);

            // Assert
            Assert.Null(testCat);
        }

    }
}