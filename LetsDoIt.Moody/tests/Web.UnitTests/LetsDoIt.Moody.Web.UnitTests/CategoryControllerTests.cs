using Moq;
using System;
using LetsDoIt.Moody.Application.Services;
using LetsDoIt.Moody.Application.Services.CategoryFolder;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using Application;
    using Controllers;
    using LetsDoIt.Moody.Application.Category;

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
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Update_WhenNameIsMissing_ShouldThrownAnArgumentException(string name)
        {
            // act 
            Action action = () => _testing.Update(1, name, 1, null);

            //assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Update_WhenImageIsMissing_ShouldThrownAnArgumentException()
        {
            // act 
            Action action = () => _testing.Update(1, "name", 1, null);

            //assert
            Assert.Throws<ArgumentException>(action);
        }

        //[Fact]
        //public void Update_ShouldUpdateCategoryInformation()
        //{
        //    // arrange
        //    var id = 1;
        //    var name = "name";
        //    var order = 1;
        //    var image = new byte[10];

        //    _mockCategoryService.Setup(cs => cs.Update(id, name, order, image));

        //    // act
        //    _testing.Update(id, name, order, image);

        //    // assert
        //    _mockCategoryService.Verify(cs => cs.Update(id, name, order, image));
        //}
    }
}
