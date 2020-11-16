using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;
using System;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using VersionHistory;
    using Domain;
    using CustomExceptions;
    using Persistance.Repositories.Base;

    public class CategoryServiceTests
    {
        private readonly CategoryService _testing;

        private readonly Mock<IEntityRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IEntityRepository<VersionHistory>> _mockVersionHistoryRepository;
        private readonly Mock<IVersionHistoryService> _mockVersionHistoryService;
        private readonly Mock<IEntityRepository<CategoryDetails>> _mockCategoryDetailsRepository;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<IEntityRepository<Category>>();
            _mockVersionHistoryRepository = new Mock<IEntityRepository<VersionHistory>>();
            _mockVersionHistoryService = new Mock<IVersionHistoryService>();
            _mockCategoryDetailsRepository = new Mock<IEntityRepository<CategoryDetails>>();

            _testing = new CategoryService(
                    _mockCategoryRepository.Object,
                    _mockCategoryDetailsRepository.Object,
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

        private void SetupGetVersionHistory(List<VersionHistory> versionHistory)
        {
            _mockVersionHistoryRepository
                            .Setup(vh => vh.Get())
                            .Returns(versionHistory.AsQueryable().BuildMockDbSet().Object);
        }

        #endregion

        [Fact]
        public async Task Should_ReturnIsUpdated_And_EmptyCategories_When_VersionNumberIsLatest()
        {
            // Arrange
            var versionNumber = "latest.versionNumber";
            var versionHistory = GetVersionHistory(versionNumber);
            SetupGetLatestVersionNumber(versionHistory);
            SetupGetCategoriesFromRepository(Enumerable.Empty<Category>().ToList());

            // Act
            var actual = await _testing.GetCategories(versionNumber);

            // Assert
            Assert.True(actual.IsUpdated);
            Assert.Equal(versionNumber, actual.VersionNumber);
            Assert.Null(actual.Categories);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_ThrowAnException_When_VersionNumberInLatestVersionIsNullOrEmptyOrWhiteSpace(string versionNumber)
        {
            //Arrange
            var versionHistory = GetVersionHistory(versionNumber);
            SetupGetLatestVersionNumber(versionHistory);

            //Act
            Func<Task<CategoryGetResult>> action = async () => await _testing.GetCategories(versionNumber);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact]
        public void Should_ThrowAnException_When_LatestVersionIsNull()
        {
            SetupGetLatestVersionNumber(null);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await _testing.GetCategories(null));
        }

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

        [Fact]
        public async Task Should_ReturnResultWithoutCategoryInfo_When_ResultIsUpdated()
        {
            // Arrange  
            var versionNumber = "good.versionNumber";

            var versionHistory = GetVersionHistory(versionNumber);
            SetupGetLatestVersionNumber(versionHistory);

            var versionHistories = new List<VersionHistory> { versionHistory };
            SetupGetVersionHistory(versionHistories);

            // Act
            var actual = await _testing.GetCategories(versionNumber);

            // Assert
            Assert.True(actual.IsUpdated);
            Assert.Equal(actual.VersionNumber, versionNumber);
            Assert.Null(actual.Categories);
        }

        [Fact]
        public async Task DeleteAsync_IdExists_DeleteIdAndCreateNewVersion()
        {
            // Arrange
            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetAsync(It.IsNotNull<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(category);


            await _testing.DeleteAsync(category.Id);

            _mockCategoryRepository.Verify(c => c.DeleteAsync(It.IsAny<Category>()), Times.Once);

            _mockVersionHistoryService.Verify(v => v.CreateNewVersionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_IdDoesNotExistsInTheDatabase_ThrowsObjectNotFoundException()
        {
            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == It.IsAny<int>()));

            async Task Test() => await _testing.DeleteAsync(category.Id);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }

        [Fact]
        public async Task InsertAsync_GivenNoException_ShouldInvokeRepositoryAddAsyncAndInvokeVersion()
        {
            var name = "asd";
            var order = 5;
            byte[] byteImage = { 80, 65, 78, 75, 65, 74 };

            await _testing.InsertAsync(name, order, byteImage);

            _mockCategoryRepository.Verify(ur =>
                ur.AddAsync(It.Is<Category>(x => x.Name == name && x.Order == order && x.Image == byteImage))
                , Times.Once);

            _mockVersionHistoryService.Verify(ur =>
                ur.CreateNewVersionAsync(), Times.Once);
        }

        [Fact]
        public async Task InsertCategoryDetailsAsync_NoException_ShouldInvokeRepositoryAddAsyncAndInvokeVersion()
        {
            var categoryId = 1;            
            var order = 5;
            var image = "cGxlYXN1cmUu";

            await _testing.InsertCategoryDetailsAsync(categoryId, order, image);

            _mockCategoryDetailsRepository.Verify(cd => cd.AddAsync(It.IsAny<CategoryDetails>()),
                Times.Once);

            _mockVersionHistoryService.Verify(v =>
                v.CreateNewVersionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryDetailsAsync_IdExists_DeleteIdAndCreateNewVersion()
        {
            var categoryDetail = new CategoryDetails
            {
                Id = 3
            };

            _mockCategoryDetailsRepository
                .Setup(repository => repository.GetAsync(It.IsNotNull<Expression<Func<CategoryDetails, bool>>>()))
                .ReturnsAsync(categoryDetail);


            await _testing.DeleteCategoryDetailsAsync(categoryDetail.Id);

            _mockCategoryDetailsRepository.Verify(c => c.DeleteAsync(It.IsAny<CategoryDetails>()), Times.Once);

            _mockVersionHistoryService.Verify(v => v.CreateNewVersionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryDetailsAsync_IdDoesNotExistsInTheDatabase_ThrowsObjectNotFoundException()
        {
            var categoryDetail = new CategoryDetails
            {
                Id = 3
            };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == It.IsAny<int>()));

            async Task Test() => await _testing.DeleteAsync(categoryDetail.Id);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }
    }
}