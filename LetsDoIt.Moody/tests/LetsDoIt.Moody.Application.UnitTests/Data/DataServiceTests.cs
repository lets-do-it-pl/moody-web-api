using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.DataService
{
    using Data;
    using Persistence;

    public class DataServiceTests
    {

        private readonly Mock<IApplicationContext> _mockApplicationContext;
        private readonly IDataService _testing;

        public DataServiceTests()
        {
            _mockApplicationContext = new Mock<IApplicationContext>();
            _testing = new DataService(_mockApplicationContext.Object);
        }

        [Fact]

        public async Task GetGeneralSearchResultAsync_ShouldThrownNullReferenceException_WhenSearchKeyIsNull()
        {
            string searchKey = null;

            async Task Test() => await _testing.SpGetGeneralSearchResultAsync(searchKey);

            await Assert.ThrowsAsync<NullReferenceException>(Test);
        }

    }
}