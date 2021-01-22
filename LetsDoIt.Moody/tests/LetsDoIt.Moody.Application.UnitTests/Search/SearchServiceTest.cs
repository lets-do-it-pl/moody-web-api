using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LetsDoIt.Moody.Application.Data;
using LetsDoIt.Moody.Application.Search;
using LetsDoIt.Moody.Persistence.Repositories.Base;
using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Search
{
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
            var result = await _testing.GetGeneralSearchResultAsync(searchKey);

            Assert.Equal(Enumerable.Empty<SpGetGeneralSearchResult>(),result );
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldCallSpGetGeneralSearchResultAsync()
        {
            var searchKey = "s";

            _mockDataService.Verify(sk => sk.SpGetGeneralSearchResultAsync(searchKey.ToLower()), Times.Once);
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

            _mockDataService.Setup(sk => sk.SpGetGeneralSearchResultAsync(searchKey)).ReturnsAsync(list);

            var result = await _testing.GetGeneralSearchResultAsync(searchKey);
            
            Assert.Equal(list,result);
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldCallSpGetGeneralSearchResultWithLowerCaseParameter()
        {
            var searchKey = "S";
            
            var result = await _testing.GetGeneralSearchResultAsync(searchKey);

            _mockDataService.Verify(ds=>ds.SpGetGeneralSearchResultAsync(searchKey.ToLower()),Times.Once);
        }
    }
}
