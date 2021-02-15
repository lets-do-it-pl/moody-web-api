using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Search
{
    using LetsDoIt.Moody.Application.Data;
    using LetsDoIt.Moody.Application.Search;
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;

    public class SearchServiceTest
    {
        private readonly SearchService _testing;
        private readonly Mock<IDataService> _mockDataService;

        public SearchServiceTest()
        {
            _mockDataService = new Mock<IDataService>();
            _testing = new SearchService(_mockDataService.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task GetGeneralSearchResultAsync_ShouldReturnEmptyList_WhenSearchKeyIsNullOrWhiteSpace(string searchKey)
        {
            var actual = await _testing.GetGeneralSearchResultAsync(searchKey);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_WhenAValidSearchKeyEntered_ShouldReturnListOfResult()
        {
            var searchKey = "s";

            var list = new List<SpGetGeneralSearchResult>
            {
                new SpGetGeneralSearchResult(),
                new SpGetGeneralSearchResult(),
                new SpGetGeneralSearchResult()
            };

            _mockDataService
                .Setup(ds => ds.GetGeneralSearchResultAsync(searchKey))
                .ReturnsAsync(list);

            var actual = await _testing.GetGeneralSearchResultAsync(searchKey);

            actual.Should().NotBeNullOrEmpty();
            actual.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldCallSpGetGeneralSearchResultWithLowerCaseParameter()
        {
            var searchKey = "S";

            await _testing.GetGeneralSearchResultAsync(searchKey);

            _mockDataService.Verify(ds => ds.GetGeneralSearchResultAsync(searchKey.ToLower()), Times.Once);
        }
    }
}
