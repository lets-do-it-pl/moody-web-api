using Moq;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using Application.VersionHistory;
    using Domain;
    using Persistance.Repositories.Base;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using MockQueryable.Moq;
    public class CategoryServiceTests
    {
        private readonly CategoryService _testing;
        private readonly Mock<IEntityRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IEntityRepository<VersionHistory>> _mockVersionHistoryRepository;
        private readonly Mock<IVersionHistoryService> _mockVersionHistoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<IEntityRepository<Category>>();
            _mockVersionHistoryRepository = new Mock<IEntityRepository<VersionHistory>>();
            _mockVersionHistoryService = new Mock<IVersionHistoryService>();

            _testing = new CategoryService(
                    _mockCategoryRepository.Object,
                    _mockVersionHistoryRepository.Object,
                    _mockVersionHistoryService.Object);
        }

        private CategoryGetResult GetCategory(string versionNumber)
        {
            return new CategoryGetResult
            {
                VersionNumber = "aaa",
                IsUpdated = false,
                Categories = new List<Category>()

            };
        }

        [Theory]
        [InlineData(null)]
        public async Task Should_ReturnCategoryGetResult_When_VersionNumberIsMissing(string versionNumber)
        {
            // Arrange
            var result = GetCategory(versionNumber);
            _mockCategoryRepository.Setup(cr => cr.GetListAsync()).Returns(result);

            // Act
            var actual = await _testing.GetCategories(versionNumber);

            // Assert
            Assert.IsType<CategoryGetResult>(actual);
        }

        [Fact]
        public async Task Should_ReturnResultWithoutCategoryInfo_When_ResultIsUpdated()
        {
            // Arrange
            var versionNumber = "good.versionNumber";
            var vHistory = new List<VersionHistory> { 
                new VersionHistory { VersionNumber = versionNumber } 
            };  
            _mockVersionHistoryRepository.Setup(vh => vh.Get()).Returns(vHistory.AsQueryable().BuildMockDbSet().Object);

            // Act
            var actual = await _testing.GetCategories(versionNumber);

            // Assert
            Assert.True(actual.IsUpdated);
            Assert.Equal(actual.VersionNumber, versionNumber);
            Assert.Empty(actual.Categories);
        }

        [Fact]
        public async Task Should_ReturnResultWithCategoryInfo_When_ResultIsNotUpdated()
        {
            var latestVersionNumber = "latest.VersionNumber";

            var versionNumber = "good.versionNumber";
            var vHistory = new List<VersionHistory> {
                new VersionHistory { VersionNumber = versionNumber }
            };

            // Arrange
            _mockVersionHistoryRepository.Setup(vh => vh.Get()).Returns(vHistory.AsQueryable().BuildMockDbSet().Object);

            // Act
            var actual = await _testing.GetCategories(versionNumber);

            // Assert
            Assert.False(actual.IsUpdated);
            Assert.NotEqual(actual.VersionNumber, latestVersionNumber);
            Assert.NotEmpty(actual.Categories);

        }
    }
}
