using System;
using System.Net.Mime;
using System.Threading.Tasks;
using LetsDoIt.Moody.Web.Entities.Requests;
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
        [Fact]
        public async Task DeleteAsync_GivenNoException_ShouldInvokeRepositoryAddAsync()
        {
            var request = new CategoryInsertRequest
            {
                Id = 1
            };
            await _testing.DeleteAsync(request.Id);
            _mockCategoryRepository.Verify(ur =>
                   ur.AddAsync(It.Is<Category>(c => c.Id == request.Id))
               );

        }

        [Fact]
        public async Task DeleteAsync_GivenNoException_ShouldInvokeVersionHistory()
        {
            var request = new CategoryDeleteRequest
            {
                Id = 1
            };

            await _testing.DeleteAsync(request.Id);

            _mockVersionHistoryService.Verify(ur =>
                ur.CreateNewVersionAsync()
            );
        }
    }


}