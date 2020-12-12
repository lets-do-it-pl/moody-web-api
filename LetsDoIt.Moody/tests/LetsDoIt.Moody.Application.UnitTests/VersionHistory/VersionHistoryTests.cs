using LetsDoIt.Moody.Application.VersionHistory;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.VersionHistory
{
    using Persistence.Repositories.Base;
    using Persistence.Entities;

    public class VersionHistoryTests
    {
        private readonly IVersionHistoryService _testing;
        private readonly Mock<IRepository<VersionHistory>> _mockVersionHistoryRepository;

        public VersionHistoryTests()
        {
            _mockVersionHistoryRepository = new Mock<IRepository<VersionHistory>>();

            _testing = new VersionHistoryService(_mockVersionHistoryRepository.Object);
        }

        [Fact]
        public async Task CreateNewVersionAsync_ShouldCallVersionHistoryRepositoryAddAsync()
        {
          await   _testing.CreateNewVersionAsync();

          _mockVersionHistoryRepository.Verify(vhr=>vhr.AddAsync(It.IsAny<VersionHistory>()),Times.Once);

        }

        [Fact]
        public async Task GetLatestVersionNumberAsync_ShouldCallVersionHistoryRepository()
        {
            var versions = new List<VersionHistory>()
            {
                new VersionHistory
                {
                    CreatedDate = DateTime.UtcNow,
                    Id = 123,
                    VersionNumber = "random",
                }
            };

            _mockVersionHistoryRepository.Setup(repo => repo.Get()).
             Returns(versions.AsQueryable().BuildMockDbSet().Object);

            await _testing.GetLatestVersionNumberAsync();

            _mockVersionHistoryRepository.Verify(vhr => vhr.Get(),Times.Once);
        } 
        
        [Fact]
        public async Task GetLatestVersionNumberAsync_WhenReturnsNull_ShouldThrowException()
        {
            var versions = new List<VersionHistory>();

            _mockVersionHistoryRepository.Setup(repo => repo.Get()).
                Returns(versions.AsQueryable().BuildMockDbSet().Object);

            async Task Test() => await _testing.GetLatestVersionNumberAsync();

            await Assert.ThrowsAsync<Exception>(Test);
        }  

        [Fact]
        public async Task GetLatestVersionNumberAsyncl_ShouldReturnLatestVersionHistory()
        {
            var versions = new List<VersionHistory>()
            {
                new VersionHistory
                {
                    CreatedDate = DateTime.UtcNow.Subtract(new TimeSpan(9999)),
                    Id = 1,
                    VersionNumber = "1"
                },
                new VersionHistory
                {
                    CreatedDate = DateTime.UtcNow,
                    Id = 2,
                    VersionNumber = "2"
                },
                new VersionHistory
                {
                    CreatedDate = DateTime.UtcNow.Subtract(new TimeSpan(1000)),
                    Id = 3,
                    VersionNumber = "3"
                }
            };

            _mockVersionHistoryRepository.Setup(repo => repo.Get()).
                Returns(versions.AsQueryable().BuildMockDbSet().Object);

            var latestVersion = await _testing.GetLatestVersionNumberAsync();

            Assert.Equal(2,latestVersion.Id);
        }
    }
}
