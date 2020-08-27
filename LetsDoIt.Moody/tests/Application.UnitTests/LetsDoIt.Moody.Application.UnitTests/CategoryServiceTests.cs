using Moq;
using Xunit;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using VersionHistory;
    using Domain;
    using CustomExceptions;
    using Persistance.Repositories.Base;
    using System;
    using System.Linq.Expressions;

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

        #endregion

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

            Action action = async () => await _testing.DeleteAsync(category.Id);

            async Task Test() => await _testing.DeleteAsync(category.Id);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }

    }
}