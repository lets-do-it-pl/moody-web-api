using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LetsDoIt.Moody.Application.Constants;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Search
{
    using Data;
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
        [InlineData(UserTypeConstants.Admin, null)]
        [InlineData(UserTypeConstants.Admin, " ")]
        [InlineData(UserTypeConstants.Standard, null)]
        [InlineData(UserTypeConstants.Standard, " ")]
        public async Task GetGeneralSearchResultAsync_ShouldReturnEmptyList_WhenSearchKeyIsNullOrWhiteSpace(string userType,string searchKey)
        {


            var actual = await _testing.GetGeneralSearchResultAsync(userType, searchKey);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_WhenAValidSearchKeyEntered_ShouldReturnListOfCategoryAndUser()
        {
            var searchKey = "s";

            var userType = UserTypeConstants.Admin;

            var list = new List<SpGetGeneralSearchResult>
            {
                new SpGetGeneralSearchResult(),
                new SpGetGeneralSearchResult(),
                new SpGetGeneralSearchResult()
            };

            _mockDataService
                .Setup(ds => ds.GetGeneralSearchResultAsync(searchKey))
                .ReturnsAsync(list);

            var actual = await _testing.GetGeneralSearchResultAsync(userType, searchKey);

            actual.Should().NotBeNullOrEmpty();
            actual.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldCallSpGetGeneralSearchResultWithLowerCaseParameterForAdmins()
        {
            var searchKey = "S";

            var userType = UserTypeConstants.Admin;

            await _testing.GetGeneralSearchResultAsync(userType, searchKey);

            _mockDataService.Verify(ds => ds.GetGeneralSearchResultAsync(searchKey.ToLower()), Times.Once);
        }
        [Fact]
        public async Task GetGeneralSearchResultAsync_WhenAValidSearchKeyEntered_ShouldReturnListOfCategory()
        {
            var searchKey = "s";

            var userType = UserTypeConstants.Standard;

            var list = new List<SpGetGeneralSearchResult>
            {
                new SpGetGeneralSearchResult(),
                new SpGetGeneralSearchResult(),
                new SpGetGeneralSearchResult()
            };

            _mockDataService
                .Setup(ds => ds.GetGeneralSearchResultAsync(searchKey))
                .ReturnsAsync(list);

            var actual = await _testing.GetGeneralSearchResultAsync(userType, searchKey);

            actual.Should().NotBeNullOrEmpty();
            actual.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldCallSpGetGeneralSearchResultWithLowerCaseParameterForStandartUser()
        {
            var searchKey = "S";

            var userType = UserTypeConstants.Admin;

            await _testing.GetGeneralSearchResultAsync(userType, searchKey);

            _mockDataService.Verify(ds => ds.GetGeneralSearchResultAsync(searchKey.ToLower()), Times.Once);
        }
    }
}
