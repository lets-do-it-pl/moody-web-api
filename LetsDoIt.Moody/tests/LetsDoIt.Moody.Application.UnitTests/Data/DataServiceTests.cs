using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using EntityFrameworkExtras.EFCore;
using LetsDoIt.Moody.Persistence.StoredProcedures;
using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.DataService
{
    using Data;
    using Persistence;

    public class DataServiceTests
    {
        private readonly DataService _testing;
        private readonly Mock<IApplicationContext> _mockApplicationContext;

        public DataServiceTests()
        {
            _mockApplicationContext = new Mock<IApplicationContext>();
            _testing = new DataService(_mockApplicationContext.Object);
        }



        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldThrowNullReferenceException_WhenSearchKeyIsNull()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => _testing.GetGeneralSearchResultAsync(""));

        }

        [Fact]
        public async Task GetGeneralSearchResultAsync_ShouldReturnSpGetResult_WhenResultExist()
        {
            string searchKey = "salih";
            var input = new SpGeneralSearch { SearchValue = searchKey };

            _mockApplicationContext.Setup(x =>
                    x.Database.ExecuteStoredProcedureAsync<SpGetGeneralSearchResult>(input, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(GetSampleSearchResult());

            var actual = _testing.GetGeneralSearchResultAsync(searchKey).Result.ToList();
            var expectedList = GetSampleSearchResult().ToList();

            Assert.Equal(expectedList[0].Name, actual[0].Name);

        }

        private IEnumerable<SpGetGeneralSearchResult> GetSampleSearchResult()
        {
            IEnumerable<SpGetGeneralSearchResult> list = new List<SpGetGeneralSearchResult>
            {
                new SpGetGeneralSearchResult
                {
                    Id = 1,
                    Name = "salih",
                    Type = "A"
                },
                new SpGetGeneralSearchResult
                {
                    Id = 1,
                    Name = "burak",
                    Type = "A"
                },
                new SpGetGeneralSearchResult
                {
                    Id = 1,
                    Name = "Ali",
                    Type = "A"
                }
            };
            return list;
        }


    }
}