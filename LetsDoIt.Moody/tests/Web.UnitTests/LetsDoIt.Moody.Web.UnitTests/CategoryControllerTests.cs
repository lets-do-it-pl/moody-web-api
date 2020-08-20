using System;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{
    using Application.Category;
    using Application.CustomExceptions;
    using Web.Controllers;    
    using Web.Entities.Requests;       

    public class CategoryControllerTests
    {
        private readonly CategoryController _testing;
        private readonly Mock<ICategoryService> _mockCategoryService;

        #region SetUp & Helpers

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _testing = new CategoryController(_mockCategoryService.Object);
        }

        
        [Fact]
        public async Task GIVEN_ThereIsAnDeleteRequestWithoutId_WHEN_DeletingTheRequest_THEN_ShouldGetBadRequest()
        {
            CategoryDeleteRequest deleteRequest = null;

            //Act
            var actual = await _testing.Delete(deleteRequest);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }


        [Fact]
        public async Task GIVEN_ThereIsAnDeleteRequest_WHEN_DeletingTheRequest_THEN_ShouldGetOkResult()
        {
            CategoryDeleteRequest deleteRequest = new CategoryDeleteRequest
            {
                Id = 1
            };

            //Act
            var actual = await _testing.Delete(deleteRequest);

            //Assert
            Assert.IsType<OkResult>(actual);
        }
        [Fact]
        public async Task GIVEN_ThereIsAnDeleteRequest_WHEN_DeletingACategory_THen_ShouldReturnOkResultAndCallServiceOnce()
        {
            //Arrange
            var request = new CategoryDeleteRequest
            {
              Id =1
            };
            
            //Act
          
            var actual = await _testing.Delete(request);

            //Assert
            _mockCategoryService
                .Verify(service =>
                        service.DeleteAsync(
                            request.Id)
                    );
        }

    }
}