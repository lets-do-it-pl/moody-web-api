using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using Application.VersionHistory;
    using Domain;
    using Persistance.Repositories.Base;

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

        #region SetUp & Helpers

        private List<Category> GetCategories() => new List<Category>
            {
                new Category{ Name = "Category1"},
                new Category{ Name = "Category2"}
            };

        private void SetupGetCategoriesFromRepository(List<Category> categories)
        {
            _mockCategoryRepository
                            .Setup(repository => repository.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                            .ReturnsAsync(categories);
        }

        private void SetupGetLatestVersionNumber(VersionHistory versionHistory)
        {
            _mockVersionHistoryService
                            .Setup(service => service.GetLatestVersionNumberAsync())
                            .ReturnsAsync(versionHistory);
        }

        private VersionHistory GetVersionHistory(string versionNumber) =>
            new VersionHistory
            {
                VersionNumber = versionNumber
            };

        #endregion

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("old.versionNumber")]
        public async Task Should_ReturnCategoryGetResult_When_VersionNumberIsNotLatest(string versionNumber)
        {
            // Arrange
            var categories = GetCategories();
            var versionHistory = GetVersionHistory("latest.versionNumber");
            SetupGetLatestVersionNumber(versionHistory);
            SetupGetCategoriesFromRepository(categories);

            // Act
            var actual = await _testing.GetCategories(versionNumber);

            // Assert
            Assert.False(actual.IsUpdated);
            Assert.Equal(categories.Count, actual.Categories.Count());
            Assert.Equal(categories, actual.Categories);
        }

        //[Fact]
        //public async Task Should_ReturnResultWithoutCategoryInfo_When_ResultIsUpdated()
        //{
        //    // Arrange // abi logic version history service te sadece bir soru 
        //    var versionNumber = "good.versionNumber";
        //    var versionHistory = new List<VersionHistory> {
        //        new VersionHistory { VersionNumber = versionNumber }
        //    };
        //    _mockVersionHistoryRepository.Setup(vh => vh.Get()).Returns(versionHistory.AsQueryable().BuildMockDbSet().Object);

        //    // Act
        //    var actual = await _testingVersionHistory.GetCategories(versionNumber);

        //    // Assert
        //    Assert.True(actual.IsUpdated);
        //    Assert.Equal(actual.VersionNumber, versionNumber);
        //    Assert.Empty(actual.Categories);
        //}

        //[Fact]
        //public async Task Should_ReturnResultWithCategoryInfo_When_ResultIsNotUpdated()
        //{
        //    var latestVersionNumber = "latest.VersionNumber";

        //    var versionNumber = "good.versionNumber";
        //    var versionHistory = new List<VersionHistory> {
        //        new VersionHistory { VersionNumber = versionNumber }
        //    };

        //    // Arrange
        //    _mockVersionHistoryRepository.Setup(vh => vh.Get()).Returns(versionHistory.AsQueryable().BuildMockDbSet().Object);

        //    // Act
        //    var actual = await _testingVersionHistory.GetCategories(versionNumber);

        //    // Assert
        //    Assert.False(actual.IsUpdated);
        //    Assert.NotEqual(actual.VersionNumber, latestVersionNumber);
        //    Assert.NotEmpty(actual.Categories);

        //}
    }
}
