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

        [Theory]
        [InlineData(" ")]
        public async Task Should_ReturnNoContent_When_CategoryResultIsNull(string versionNumber)
        {
         // _mockCategoryService.Setup(cs => cs.GetCategories(It.IsAny<string>()).Result).Equals(null);
            var result = await _testing.GetCategories(versionNumber);
           
            Assert.IsType<ObjectResult>(result);
            var objectResponse = result as ObjectResult; //Cast to desired type

            Assert.Equal((int)HttpStatusCode.NoContent, objectResponse.StatusCode);

        }
        // mock objeyi olustur - sonra davranmasini istedigin gibi setup et -
        // sonra setup ettigin metodu cagir(bir degere atanabilir) ve en son assertle kontrol et - cagirilmis mi , esit mi, type doru mu etc
      
        [Fact]
        public async Task Should_ReturnNoContent_When_CategoryResultIsNotUpdatedAndCategoriesIsNull()
        {
            //Arrange
            var versionNumber = "abc";
            // could not find the way to check two conditions
            _mockCategoryService.Setup(cs => cs.GetCategories(It.IsAny<string>()).Result.IsUpdated).Returns(false);
            
            //Act
            var actual = await _testing.GetCategories(versionNumber);
           
            //Assert

        }

        //[Theory]
        //[InlineData("aaa")]
        //public async Task Should_ReturnResultWithoutCategoryInfo_When_ResultIsUpdated(string versionNumber)
        //{
        //    //Arrange
        //    DateTime date1 = new DateTime(2014, 07, 25, 13, 30, 01);
        //    DateTime date2 = new DateTime(2014, 07, 25, 13, 30, 00);


        //    _mockCategoryService.Setup(x => x.GetCategories(versionNumber)).Returns();

        //    //Act

        //    //Assert

        //}

        //[Theory]
        //[InlineData(null)]
        //[InlineData("")]
        //[InlineData(" ")]
        //public async Task Insert_WhenNameIsMissing_ShouldThrownAnArgumentException(string name)
        //{
        //    // arrange
        //    byte[] arr1 = { 0, 100, 120, 210, 255 };

        //    var insertRequest = new CategoryInsertRequest
        //    {
        //        Name = name,
        //        Order = 5,
        //        Image = arr1
        //    };

        //    // act
        //    async Task Action() => await _testing.Insert(insertRequest);

        //    //assert
        //    await Assert.ThrowsAsync<ArgumentException>(Action);
        //}


        //[Fact]
        //public async Task Insert_WhenImageisMissing_ShouldThrownAnArgumentException()
        //{
        //    //arrange
        //    var insertRequest = new CategoryInsertRequest
        //    {
        //        Name = "sdfasdf",
        //        Order = 5,
        //        Image = null
        //    };

        //    // act
        //    async Task Action() => await _testing.Insert(insertRequest);

        //    //assert
        //    await Assert.ThrowsAsync<ArgumentException>(Action);
        //}

        //[Fact]
        //public async Task SaveUserAsync_ShouldSaveUserInformation()
        //{
        //    // arrange
        //    byte[] arr1 = { 0, 100, 120, 210, 255 };

        //    var insertRequest = new CategoryInsertRequest
        //    {
        //        Name = "test",
        //        Order = 5,
        //        Image = arr1
        //    };


        //    _mockCategoryService.Setup(cs =>
        //        cs.InsertAsync(
        //            insertRequest.Name,
        //            insertRequest.Order,
        //            insertRequest.Image));

        //    // act
        //    await _testing.Insert(insertRequest);

        //    // assert
        //    _mockCategoryService.Verify(cs =>
        //        cs.InsertAsync(
        //            insertRequest.Name,
        //            insertRequest.Order,
        //            insertRequest.Image),
        //        Times.Once);
        //}

    }
}
