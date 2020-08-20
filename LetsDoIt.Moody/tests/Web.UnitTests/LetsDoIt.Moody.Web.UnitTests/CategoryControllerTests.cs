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
        public async Task GIVEN_ThereIsAnDeleteRequest_WHEN_DeletingTheRequest_THEN_ShouldGetOkResult()
        {
            int id = 3;

            _mockCategoryService.Setup(c => c.DeleteAsync(id));

            //Act
            var actual = await _testing.Delete(id);

            //Assert
            Assert.IsType<OkResult>(actual);

        }

        [Fact]
        public async Task GIVEN_ThereIsAnDeleteRequestWithoutId_WHEN_DeletingTheRequest_THEN_ShouldGetBadRequest()
        {
            int id = 0;
            _mockCategoryService.Setup(c => c.DeleteAsync(id)).Throws(new ObjectNotFoundException("Not found"));

            //Act
            var actual = await _testing.Delete(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actual);
        }


    }
}