using LetsDoIt.Moody.Application;
using LetsDoIt.Moody.Web.Controllers;
using Moq;
using System;
using Xunit;

namespace LetsDoIt.Moody.Web.UnitTests
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _testing;
        private Mock<ICategoryService> _mockCategoryService;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _testing = new CategoryController(_mockCategoryService.Object);
        }

        [Fact]
        public void Update_WhenNameIsMissing_ShouldThrownAnArgumentExcception()
        {
            // arrange
            var id = 1;
            string name = null;
            var order = 1;

            // act 
            Action action = () => _testing.Update(id, name, order, null);

            //assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}
