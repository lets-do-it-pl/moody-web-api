using Moq;
using System;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    using Application;
    using Controllers;
    using LetsDoIt.Moody.Application.Category;
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

        //[Theory]
        //[InlineData(null)]
        //[InlineData("")]
        //[InlineData(" ")]
        //public void Update_WhenNameIsMissing_ShouldThrownAnArgumentException(string name)
        //{
        //    // act 
        //    Action action = () => _testing.Update(1, name, 1, null);

        //    //assert
        //    Assert.Throws<ArgumentException>(action);
        //}

        //[Fact]
        //public void Update_WhenImageIsMissing_ShouldThrownAnArgumentException()
        //{
        //    // act 
        //    Action action = () => _testing.Update(1, "name", 1, null);

        //    //assert
        //    Assert.Throws<ArgumentException>(action);
        //}

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Insert_WhenNameIsMissing_ShouldThrownAnArgumentException(string name)
        {
            // arrange
            byte[] arr1 = { 0, 100, 120, 210, 255 };

           // act
            async Task Action() => await _testing.Insert(name,5,arr1);

            //assert
            await Assert.ThrowsAsync<ArgumentException>(Action);
        }


        [Fact]
        public async Task Insert_WhenImageisMissing_ShouldThrownAnArgumentException()
        { 
            // act
            async Task Action() => await _testing.Insert("sdfasdf", 5, null);

            //assert
            await Assert.ThrowsAsync<ArgumentException>(Action);
        }



        [Fact]
        public async Task SaveUserAsync_ShouldSaveUserInformation()
        {
            // arrange
            var name = "Deneme";
            int order = 5;
            byte[] arr1 = { 0, 100, 120, 210, 255 };

            _mockCategoryService.Setup(cs => cs.InsertAsync(name,order,arr1));

            // act
            await _testing.Insert(name,order,arr1);

            // assert
            _mockCategoryService.Verify(cs => cs.InsertAsync(name,order,arr1), Times.Once);
        }

    }
}
