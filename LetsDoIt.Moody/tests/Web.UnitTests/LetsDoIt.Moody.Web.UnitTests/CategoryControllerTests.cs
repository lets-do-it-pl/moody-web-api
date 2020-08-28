using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests.Controllers
{    
    using Application.Category;
    using Application.CustomExceptions;
    using Application.Category;
    using Domain;
    using Web.Entities.Responses;
    using Web.Controllers;  
    
    public class CategoryControllerTests
    {
        private readonly CategoryController _testing;
        private readonly Mock<ICategoryService> _mockCategoryService;

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
      
        [Theory]
        [InlineData("asd", "asd")]
        [InlineData("asd ", "asd")]
        [InlineData("asd  ", "asd")]
        [InlineData("   asd", "asd")]
        [InlineData("  asd", "asd")]
        public async Task Should_BeTrimmedVersionNumber_When_VersionNumberWithSpaces(string versionNumber, string expected)
        {
            //Act
            await _testing.GetCategories(versionNumber);

            //Assert
            _mockCategoryService.Verify(service => service.GetCategories(expected), Times.Once);
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
      
        [Fact]
        public async Task Should_ReturnNoContent_When_CategoryResultIsNull()
        {
            var versionNumber = " ";

             _mockCategoryService
                .Setup(cs => cs.GetCategories(It.IsAny<string>()))
                .ReturnsAsync(new CategoryGetResult());

            var result = await _testing.GetCategories(versionNumber);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task Should_ReturnNoContent_When_CategoryResultIsNotUpdatedAndCategoriesIsNull()
        {
            //arrange
            var versionNumber = "good.VersionNumber";

            var categoryResult = new CategoryGetResult
            {
                IsUpdated = false,
                VersionNumber = "good.VersionNumber",
                Categories = Enumerable.Empty<Category>()
            };

            _mockCategoryService
                 .Setup(service => service.GetCategories(It.IsAny<string>()))
                 .ReturnsAsync(categoryResult);

            //act
            var actual = await _testing.GetCategories(versionNumber);

            //Assert
            Assert.NotNull(actual);
            Assert.IsType<ActionResult<CategoryResponse>>(actual);

            Assert.False(actual.Value.IsUpdated);
            Assert.Empty(actual.Value.Categories); 
        }
    }
}
