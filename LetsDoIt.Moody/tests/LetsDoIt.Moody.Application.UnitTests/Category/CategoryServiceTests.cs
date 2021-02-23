using LetsDoIt.Moody.Application.VersionHistory;
using LetsDoIt.Moody.Persistence.Repositories.Category;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using Xunit;
using LazyCache.Mocks;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using CustomExceptions;
    using Persistence.Entities;
    using Persistence.Repositories.Base;

    public class CategoryServiceTests
    {
        private readonly CategoryService _testing;

        private readonly MockCachingService _mockCache;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IParameterItemService> _mockParameterItemService;
        private readonly Mock<IRepository<CategoryDetail>> _mockCategoryDetailsRepository;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockParameterItemService = new Mock<IParameterItemService>();
            _mockCategoryDetailsRepository = new Mock<IRepository<CategoryDetail>>();
            _mockCache = new MockCachingService();

            _testing = new CategoryService(
                    _mockCategoryRepository.Object,
                    _mockCategoryDetailsRepository.Object,
                    _mockParameterItemService.Object,
                    _mockCache);
        }

        #region GetCategoriesWithDetail

        [Fact]
        public async Task GetCategoriesWithDetails_WhenLatestVersionNumberIsGiven_ShouldReturnIsUpdatedTrueAndEmptyCategories()
        {
            var parameterItem = new ParameterItem
            {
                ParameterKey = "CategpryLatestVersion",
                ParameterValue = "latestVersion"
            };

            _mockParameterItemService.Setup(pi => pi.GetLatestVersionNumberAsync())
                .ReturnsAsync(parameterItem);

            var actual = await _testing.GetCategoriesWithDetailsAsync("latestVersion");

            Assert.True(actual.IsUpdated);
            Assert.Equal("latestVersion", actual.VersionNumber);
            Assert.Null(actual.Categories);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetCategoriesWithDetails_ShouldThrowAnException_WhenLatestVersionHistoryIsNulEmptyOrWhiteSpace(string versionNumber)
        {
            _mockParameterItemService.Setup(pi => pi.GetLatestVersionNumberAsync())
                .ReturnsAsync(new ParameterItem
                {
                    ParameterKey = "LatestVersion",
                    ParameterValue = versionNumber
                });

            async Task Test() => await _testing.GetCategoriesWithDetailsAsync("filler");

            await Assert.ThrowsAsync<ArgumentException>(Test);
        }


        [Fact]
        public void GetCategoriesWithDetails_ShouldThrowAnException_WhenLatestVersionHistoryIsNull()
        {

            _mockParameterItemService.Setup(vhs => vhs.GetLatestVersionNumberAsync())
                .ReturnsAsync((ParameterItem)null);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await _testing.GetCategoriesWithDetailsAsync(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("oldVersionNumber")]
        public async Task GetCategoriesWithDetails_ShouldReturnCategoriesList_WhenVersionNumberIsNotLatest(string versionNumber)
        {
            var categories = new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            };

            _mockCategoryRepository.Setup(cp => cp.GetListWithDetailsAsync()).ReturnsAsync(categories);

            _mockParameterItemService.Setup(vhs => vhs.GetLatestVersionNumberAsync())
                .ReturnsAsync(new ParameterItem
                {
                    ParameterKey = "CategoryLatestVersion",
                    ParameterValue = "latestVersionNumber"
                });

            var actual = await _testing.GetCategoriesWithDetailsAsync(versionNumber);

            Assert.False(actual.IsUpdated);
            Assert.NotNull(actual.Categories);
            Assert.Equal(categories.Count, actual.Categories.Count());
        }
        #endregion

        #region GetCategories

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnCategoriesList()
        {
            var categories = new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            };

            _mockCategoryRepository.Setup(cp => cp.GetListAsync(It.IsNotNull<Expression<Func<Category, bool>>>(), null))
                .ReturnsAsync(categories);

            var actual = await _testing.GetCategoriesAsync();

            Assert.NotNull(actual);
            Assert.Equal(categories.Count(), actual.Count());
        }

        #endregion

        #region GetCategoryDetails

        [Fact]
        public async Task GetCategoryDetailsAsync_ShouldReturnCategoryDetailsList()
        {
            var categoryId = 1;

            var categoryDetails = new List<CategoryDetail>
            {
                new CategoryDetail(),
                new CategoryDetail(),
                new CategoryDetail()
            };

            _mockCategoryDetailsRepository.Setup(cp => cp.GetListAsync(
                It.IsNotNull<Expression<Func<CategoryDetail, bool>>>(), null))
                .ReturnsAsync(categoryDetails);

            var actual = await _testing.GetCategoryDetailsAsync(categoryId);

            Assert.NotNull(actual);
            Assert.Equal(categoryDetails.Count(), actual.Count());
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_WhenCategoryIdExists_ShouldDeleteCategoryAndUpdateTheVersionNumber()
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

            _mockParameterItemService.Verify(p => p.UpdateVersionNumberAsync(It.IsAny<int>()), Times.Once);

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
        public async Task InsertAsync_ShouldCallRepositoryAndUpdateTheVersionNumber()
        {
            var name = "asd";
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };
            var userId = 1;

            await _testing.InsertAsync(name, byteImage, userId);

            _mockCategoryRepository.Verify(ur =>
                    ur.AddAsync(It.Is<Category>(c => c.Name == name && c.Image == byteImage && c.CreatedBy == userId))
                , Times.Once);

            _mockParameterItemService.Verify(p =>
                p.UpdateVersionNumberAsync(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region InsertCategoryDetailAsync

        [Fact]
        public async Task InsertCategoryDetailAsync_ShouldCallCategoryDetailsRepositoryAndUpdateTheVersionNumber()
        {
            var userId = 1;
            var categoryId = 1;
            var image = "YTM0NZomIzI2OTsmIzM0NTueYQ==";

            await _testing.InsertCategoryDetailAsync(categoryId, image, userId);

            _mockCategoryDetailsRepository.Verify(cdr =>
                    cdr.AddAsync(It.Is<CategoryDetail>(cd =>
                    cd.CategoryId == categoryId &&
                    cd.Image.SequenceEqual(Convert.FromBase64String(image)) &&
                    cd.CreatedBy == userId)),
                Times.Once);

            _mockParameterItemService.Verify(p =>
                p.UpdateVersionNumberAsync(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region DeleteCategoryDetailsAsync

        [Fact]
        public async Task DeleteCategoryDetailsAsync_WhenIdExists_ShouldDeleteCategoryDetailAndUpdateTheVersion()
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

            _mockParameterItemService.Verify(p =>
                 p.UpdateVersionNumberAsync(It.IsAny<int>()), Times.Once);
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

            async Task Test() => await _testing.DeleteAsync(categoryDetail.Id, userId);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }
        #endregion

        #region UpdateCategoryDetailsAsync

        [Fact]
        public async Task UpdateCategoryDetailsAsync_WhenIdExists_ShouldUpdateCategoryDetailAndUpdateTheVersion()
        {
            var userId = 1;
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };

            var categoryDetail = new CategoryDetail
            {
                Id = 3
            };

            _mockCategoryDetailsRepository
                .Setup(repository => repository.GetAsync(It.IsNotNull<Expression<Func<CategoryDetail, bool>>>()))
                .ReturnsAsync(categoryDetail);

            await _testing.UpdateCategoryDetailsAsync(categoryDetail.Id, byteImage, userId);

            _mockCategoryDetailsRepository.Verify(c =>
                c.UpdateAsync(It.Is<CategoryDetail>(cd => cd.Id == categoryDetail.Id && cd.ModifiedBy == userId)), Times.Once);

            _mockParameterItemService.Verify(p =>
                 p.UpdateVersionNumberAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryDetailsAsync_WhenCategoryDetailDoesNotExists_ShouldThrowObjectNotFoundException()
        {
            var userId = 1;
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };
            var categoryDetail = new CategoryDetail
            {
                Id = 3
            };

            _mockCategoryDetailsRepository.Setup(repo => repo.GetAsync(c => c.Id == It.IsAny<int>()))
                .ReturnsAsync((CategoryDetail)null);

            async Task Test() => await _testing.UpdateCategoryDetailsAsync(categoryDetail.Id, byteImage,userId);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_WhenCategoryIdExists_ShouldUpdateCategoryAndUpdateTheVersionNumber()
        {
            var userId = 1;
            var name = "Test";
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };

            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetAsync(It.IsNotNull<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(category);

            await _testing.UpdateAsync(category.Id, name, byteImage, userId);

            _mockCategoryRepository.Verify(c => c.UpdateAsync(It.Is<Category>(c => c.Id == category.Id && c.ModifiedBy == userId)), Times.Once);

            _mockParameterItemService.Verify(p => p.UpdateVersionNumberAsync(It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryIdDoesNotExists_ShouldThrowObjectNotFoundException()
        {
            var userId = 1;
            var name = "Test";
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };
            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsNotNull<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            async Task Test() => await _testing.UpdateAsync(category.Id, name, byteImage, userId);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }

        #endregion
    }
}