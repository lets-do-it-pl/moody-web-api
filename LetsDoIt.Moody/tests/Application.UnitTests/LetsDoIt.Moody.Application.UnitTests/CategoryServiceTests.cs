using Moq;
using Xunit;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.UnitTests.Category
{
    using Application.Category;
    using Application.VersionHistory;
    using Domain;
    using LetsDoIt.Moody.Application.CustomExceptions;
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

        [Fact]
        public async Task DeleteAsync_IdExists_DeleteIdAndCreateNewVersion()
        {
            var category = new Category
            {
                Id = 3
            };

            _mockCategoryRepository.Setup(c => c.GetAsync(id => id.Id == category.Id));

            await _testing.DeleteAsync(category.Id);

            _mockCategoryRepository.Verify(c => c.DeleteAsync(category), Times.Once);

            _mockVersionHistoryService.Verify(v => v.CreateNewVersionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_IdDoesNotExistsInTheDatabase_ThrowsObjectNotFoundException()
        {
            var category = new Category
            {
                Id = 3
            };

            async Task Test() => await _testing.DeleteAsync(category.Id);

            await Assert.ThrowsAsync<ObjectNotFoundException>(Test);
        }

    }
}