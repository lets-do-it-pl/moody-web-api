using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            Assert.ThrowsAsync<NullReferenceException>(() => _testing.GetGeneralSearchResultAsync(""));
            

        }

        //[Fact]
        //public async Task GetGeneralSearchResultAsync_ShouldReturnSpGetResult_WhenResultExist()
        //{
        //    var entity = new SpGetGeneralSearchResult
        //    {
        //        Id = 1,
        //        Name = "slaih",
        //        Type = "aa"
        //    };
        //    _mockApplicationContext.Setup(x=>x.Database.ExecuteStoredProcedureAsync<SpGetGeneralSearchResult>())
        //}

    }
}