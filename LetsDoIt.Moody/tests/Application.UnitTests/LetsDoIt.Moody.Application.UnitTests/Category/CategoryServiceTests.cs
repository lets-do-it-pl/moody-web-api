
using Moq;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using Application.VersionHistory;
    using Domain;
    using LetsDoIt.Moody.Web.Controllers;
    using Persistance.Repositories.Base;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class CategoryServiceTests
    {
        private readonly CategoryService _testing;

        private readonly Mock<IEntityRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IEntityRepository<VersionHistory>> _mockVersionHistoryRepository;
        private readonly Mock<IVersionHistoryService> _mockVersionHistoryService;

        private readonly CategoryController _testingController;
        private readonly Mock<ICategoryService> _mockCategoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<IEntityRepository<Category>>();
            _mockVersionHistoryRepository = new Mock<IEntityRepository<VersionHistory>>();
            _mockVersionHistoryService = new Mock<IVersionHistoryService>();

            _testing = new CategoryService(
                    _mockCategoryRepository.Object,
                    _mockVersionHistoryRepository.Object,
                    _mockVersionHistoryService.Object);

            _mockCategoryService = new Mock<ICategoryService>();
            _testingController = new CategoryController(_mockCategoryService.Object);
        }

        private CategoryGetResult GetCategory(string versionNumber)
        {

            List<Category> list = new List<Category>();
            IEnumerable<Category> categoryList = list;

            return new CategoryGetResult
            {
                VersionNumber = "aaa",
                IsUpdated = false,
                Categories = categoryList
            };
        }
/*
        [Fact]
        public async Task Should_ThrowsException_When_LatestVersionIsMissing()
        {
            //Arrange
            // var latestVersion = _testing.Get().OrderByDescending(vh => vh.CreateDate).FirstOrDefault();

            //Act

            //Assert

        }
*/
        [Theory]
        [InlineData(null)]
        public async Task Should_ReturnCategoryGetResult_When_VersionNumberIsMissing(string versionNumber)
        {
            //Arrange
            var result = GetCategory(versionNumber);
            _mockCategoryService.Setup(service => service.GetCategories(versionNumber)).ReturnsAsync(result);


            //Act
            var categories = await _testingController.GetCategories(versionNumber);

            //Assert
            Assert.IsType<CategoryGetResult>(categories);


        }
    }
}
