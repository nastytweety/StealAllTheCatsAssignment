using Moq;
using StealAllTheCatsAssignment.Application.IRepository;
using StealAllTheCatsAssignment.Application.Mapperly;
using StealAllTheCatsAssignment.Domain.Models;
using StealAllTheCatsAssignment.Services;

namespace StealAllTheCatsAssignment.Tests
{
    public class AppServiceTests
    {

        private readonly AppService _appService;
        private readonly Mock<IAppRepository> _appRepository = new Mock<IAppRepository>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        public AppServiceTests()
        {
            _appService = new AppService(_appRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetCatById_ShouldReturnCat_WhenCatExists()
        {
            // Arrange
            int catId = 1;
            string CatName = "Black Cat";
            var cat = new Cat { Id = catId, CatId = CatName, Created=DateTime.Now };
            _appRepository.Setup(x => x.Get(catId)).ReturnsAsync(cat);
            // Act
            var testCat = await _appService.GetCatById(catId);
            
            // Assert
            Assert.Equal(catId, testCat.Id);
            Assert.Equal(CatName, testCat.CatId);
        }

        [Fact]
        public async Task GetCatById_ShouldReturnNull_WhenCatDoesNotExist()
        {
            // Arrange
            int catId = 1;
            _appRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(()=>null);
            // Act
            var testCat = await _appService.GetCatById(catId);

            // Assert
            Assert.Null(testCat);
        }

        [Fact]
        public async Task GetCatBytag_ShouldReturnNull_WhenCatsDoesNotExist()
        {
            // Arrange
            var catTagName = "friendly";
            Cat cat = new Cat { CatId = "black" };
            List<Cat> cats = new List<Cat>();
            cats.Add(cat);
            _appRepository.Setup(x => x.GetAll()).ReturnsAsync(cats);
            _appRepository.Setup(x => x.GetTag(catTagName)).ReturnsAsync(() => null);
            // Act
            var testCat = await _appService.GetCatsByTag(catTagName);

            // Assert
            Assert.Null(testCat);

        }
    }
}