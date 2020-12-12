using LetsDoIt.Moody.Application.VersionHistory;
using LetsDoIt.Moody.Persistence.Repositories.Category;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using CustomExceptions;
    using Persistence.Entities;
    using Persistence.Repositories.Base;

    public class CategoryServiceTests
    {
        private readonly CategoryService _testing;

        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IVersionHistoryService> _mockVersionHistoryService;
        private readonly Mock<IRepository<CategoryDetail>> _mockCategoryDetailsRepository;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockVersionHistoryService = new Mock<IVersionHistoryService>();
            _mockCategoryDetailsRepository = new Mock<IRepository<CategoryDetail>>();

            _testing = new CategoryService(
                    _mockCategoryRepository.Object,
                    _mockCategoryDetailsRepository.Object,
                    _mockVersionHistoryService.Object);
        }

        #region GetCategoriesWithDetails

        [Fact]
        public async Task GetCategoriesWithDetails_WhenLatestVersionNumberIsGiven_ShouldReturnIsUpdatedTrueAndEmptyCategories()
        {
            var versionNumber = "latest.versionNumber";

            _mockVersionHistoryService.Setup(vhs => vhs.GetLatestVersionNumberAsync())
                .ReturnsAsync(new VersionHistory
                {
                    VersionNumber = versionNumber
                });

            var actual = await _testing.GetCategoriesWithDetails(versionNumber);

            Assert.True(actual.IsUpdated);
            Assert.Equal(versionNumber, actual.VersionNumber);
            Assert.Null(actual.Categories);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetCategoriesWithDetails_ShouldThrowAnException_WhenLatestVersionHistoryIsNulEmptyOrWhiteSpace(string versionNumber)
        {
            _mockVersionHistoryService.Setup(vhs => vhs.GetLatestVersionNumberAsync())
                .ReturnsAsync(new VersionHistory
                {
                    VersionNumber = versionNumber
                });

            async Task Test() => await _testing.GetCategoriesWithDetails("filler");

            await Assert.ThrowsAsync<ArgumentException>(Test);
        }


        [Fact]
        public void GetCategoriesWithDetails_ShouldThrowAnException_WhenLatestVersionHistoryIsNull()
        {

            _mockVersionHistoryService.Setup(vhs => vhs.GetLatestVersionNumberAsync())
                .ReturnsAsync((VersionHistory)null);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await _testing.GetCategoriesWithDetails(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("old.versionNumber")]
        public async Task GetCategoriesWithDetails_ShouldReturnCategoriesList_WhenVersionNumberIsNotLatest(string versionNumber)
        {
            var categories = new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            };

            _mockCategoryRepository.Setup(cp => cp.GetListWithDetailsAsync()).ReturnsAsync(categories);

            _mockVersionHistoryService.Setup(vhs => vhs.GetLatestVersionNumberAsync())
                .ReturnsAsync(new VersionHistory
                {
                    VersionNumber = "latest.versionNumber"
                });

            var actual = await _testing.GetCategoriesWithDetails(versionNumber);

            Assert.False(actual.IsUpdated);
            Assert.NotNull(actual.Categories);
            Assert.Equal(categories.Count, actual.Categories.Count());
        }
        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_WhenCategoryIdExists_ShouldDeleteCategoryAndCreateNewVersionNumber()
        {

            var userId = 1;
            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetAsync(It.IsNotNull<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(category);

            await _testing.DeleteAsync(category.Id, userId);

            _mockCategoryRepository.Verify(c => c.DeleteAsync(It.Is<Category>(c => c.Id == category.Id && c.ModifiedBy == userId)), Times.Once);

            _mockVersionHistoryService.Verify(v => v.CreateNewVersionAsync(), Times.Once);

        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryIdDoesNotExists_ShouldThrowObjectNotFoundException()
        {
            var userId = 1;
            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsNotNull<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            async Task Test() => await _testing.DeleteAsync(category.Id, userId);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }
        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_ShouldCallRepositoryAndCallVersionHistoryService()
        {
            var name = "asd";
            var order = 5;
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };
            var userId = 1;

            await _testing.InsertAsync(name, order, byteImage, userId);

            _mockCategoryRepository.Verify(ur =>
                    ur.AddAsync(It.Is<Category>(c => c.Name == name && c.Order == order && c.Image == byteImage && c.CreatedBy == userId))
                , Times.Once);

            _mockVersionHistoryService.Verify(ur =>
                ur.CreateNewVersionAsync(), Times.Once);
        }
        #endregion

        #region InsertCategoryDetailAsync

        [Fact]
        public async Task InsertCategoryDetailAsync_ShouldCallCategoryDetailsRepositoryAndCallVersionHistoryService()
        {
            var userId = 1;
            var categoryId = 1;
            var order = 5;
            var image = "YTM0NZomIzI2OTsmIzM0NTueYQ==";

            await _testing.InsertCategoryDetailAsync(categoryId, order, image, userId);

            _mockCategoryDetailsRepository.Verify(cdr =>
                    cdr.AddAsync(It.Is<CategoryDetail>(cd => cd.CategoryId == categoryId && cd.Order == order
                    && cd.Image.SequenceEqual(Convert.FromBase64String(image)) && cd.CreatedBy == userId)),
                Times.Once);

            _mockVersionHistoryService.Verify(v =>
                v.CreateNewVersionAsync(), Times.Once);
        }
        #endregion

        #region DeleteCategoryDetailsAsync

        [Fact]
        public async Task DeleteCategoryDetailsAsync_WhenIdExists_ShouldDeleteCategoryDetailAndCreateNewVersion()
        {
            var userId = 1;
            var categoryDetail = new CategoryDetail
            {
                Id = 3
            };

            _mockCategoryDetailsRepository
                .Setup(repository => repository.GetAsync(It.IsNotNull<Expression<Func<CategoryDetail, bool>>>()))
                .ReturnsAsync(categoryDetail);

            await _testing.DeleteCategoryDetailsAsync(categoryDetail.Id, userId);

            _mockCategoryDetailsRepository.Verify(c =>
                c.DeleteAsync(It.Is<CategoryDetail>(cd => cd.Id == categoryDetail.Id && cd.ModifiedBy == userId)), Times.Once);

            _mockVersionHistoryService.Verify(v => v.CreateNewVersionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryDetailsAsync_WhenCategoryDetailDoesNotExists_ShouldThrowObjectNotFoundException()
        {
            var userId = 1;
            var categoryDetail = new CategoryDetail
            {
                Id = 3
            };

            _mockCategoryDetailsRepository.Setup(repo => repo.GetAsync(c => c.Id == It.IsAny<int>()))
                .ReturnsAsync((CategoryDetail)null);

            async Task Test() => await _testing.DeleteAsync(categoryDetail.Id,userId);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }
        #endregion
    }
}