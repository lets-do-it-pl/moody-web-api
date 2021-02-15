using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Search
{
    using Data;
    using LetsDoIt.Moody.Application.Search;
    using Persistence.StoredProcedures.ResultEntities;
    public class SearchServiceTest
    {
        private readonly ISearchService _testing;
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

            actual.Should().Equal(Enumerable.Empty<SpGetGeneralSearchResult>());
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

            var actual = await _testing.GetGeneralSearchResultAsync(searchKey);
            if (actual != null)
            {
                actual.Should().BeEquivalentTo(list);
            }
           
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldCallSpGetGeneralSearchResultWithLowerCaseParameter()
        {
            var searchKey = "S";

            var actual = await _testing.GetGeneralSearchResultAsync(searchKey);

            _mockDataService.Verify(ds=>ds.GetGeneralSearchResultAsync(searchKey.ToLower()),Times.Once);

        }
    }
}
