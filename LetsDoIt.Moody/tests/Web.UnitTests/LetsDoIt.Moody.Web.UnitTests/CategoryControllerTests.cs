using Moq;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using Controllers;
    using LetsDoIt.Moody.Application.Category;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class CategoryControllerTests
    {
        private readonly CategoryController _testing;
        private readonly Mock<ICategoryService> _mockCategoryService;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _testing = new CategoryController(_mockCategoryService.Object);
        }


        [Theory]
        [InlineData("asd","asd")]
        [InlineData("asd ", "asd")]
        [InlineData("asd  ", "asd")]
        [InlineData("   asd", "asd")]
        [InlineData("  asd", "asd")]
        public async Task Should_BeTrimmedVersionNumber_When_VersionNumberWithSpaces(string versionNumber, string expected)
        {

            //Act
            var categories = await _testing.GetCategories(versionNumber);

            //Assert
            _mockCategoryService.Verify(service => service.GetCategories(expected), Times.Once);

        }

        //[Theory]
        //[InlineData(" ")]
        //public async Task Should_ReturnNoContent_When_CategoryResultIsNull(string versionNumber)
        //{
        //    // _mockCategoryService.Setup(cs => cs.GetCategories(It.IsAny<string>()).Result).Equals(null);
        //    var result = await _testing.GetCategories(versionNumber);

        //    Assert.IsType<ObjectResult>(result);
        //    var objectResponse = result as ObjectResult; //Cast to desired type

        //    Assert.Equal((int)HttpStatusCode.NoContent, objectResponse.StatusCode);

        //}


        // mock objeyi olustur - sonra davranmasini istedigin gibi setup et -
        // sonra setup ettigin metodu cagir(bir degere atanabilir) ve en son assertle kontrol et - cagirilmis mi , esit mi, type doru mu etc

        //[Fact]
        //public async Task Should_ReturnNoContent_When_CategoryResultIsNotUpdatedAndCategoriesIsNull()
        //{
        //    //Arrange
        //    var versionNumber = "abc";
        //    // could not find the way to check two conditions
        //    _mockCategoryService.Setup(cs => cs.GetCategories(It.IsAny<string>()).Result.IsUpdated).Returns(false);

        //    //Act
        //    var actual = await _testing.GetCategories(versionNumber);

        //    //Assert

        //}

    }
}
