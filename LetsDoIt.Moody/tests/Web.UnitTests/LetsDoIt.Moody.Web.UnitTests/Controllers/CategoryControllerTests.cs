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
    using Entities.Requests;

    public class CategoryControllerTests
    {
        private readonly byte[] _byteImage;
        private readonly CategoryInsertRequest _request;
        private readonly CategoryController _testing;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<ILogger<CategoryController>> _mockILogger;

        #region SetUp & Helpers

        public CategoryControllerTests()
        {
            _mockILogger = new Mock<ILogger<CategoryController>>();
            _mockCategoryService = new Mock<ICategoryService>();
            _testing = new CategoryController(_mockCategoryService.Object, _mockILogger.Object);
            _request = new CategoryInsertRequest
            {
                Name = "adsfasdf",
                Order = 5,
                Image = "USrCELxGejBZI4W/Llsvmw==\r\n"
            };
            _byteImage = Convert.FromBase64String(_request.Image);

        }

        private CategoryUpdateRequest GetCategoryUpdateRequest(
            byte[] image,
            int id = 1,
            string name = "name",
            int order = 1,
            bool isNull = false)
        {
            if (isNull)
            {
                return null;
            }

            return new CategoryUpdateRequest
            {
                Id = id,
                Name = name,
                Image = image,
                Order = order
            };
        }

        private CategoryUpdateRequest GetCategoryUpdateRequest(
            int id = 1,
            string name = "name",
            int order = 1,
            bool isNull = false)
        {
            return GetCategoryUpdateRequest(new byte[] { 1 }, id, name, order, isNull);
        }

        #endregion

        [Fact(Skip = "will be corrected")]
        public async Task GIVEN_ThereIsAnUpdateRequestWithoutName_WHEN_UpdatingACategory_THEN_ShouldGetBadRequest()
        {
            //Arrange         
            var request = GetCategoryUpdateRequest(name: null);

            //Act
            var actual = await _testing.Update(request);

            //Assert
            //Assert.IsType<BadRequestResult>(actual);
        }

        [Fact(Skip = "will be corrected")]
        public async Task GIVEN_ThereIsAnUpdateRequestWithoutImage_WHEN_UpdatingACategory_THEN_ShouldGetBadRequest()
        {
            //Arrange
            var request = GetCategoryUpdateRequest(image: null);

            //Act
            var actual = await _testing.Update(request);

            //Assert
            //Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task GIVEN_ThereIsNoUpdateRequest_WHEN_UpdatingACategory_THEN_ShouldGetBadRequest()
        {
            //Arrange
            var request = GetCategoryUpdateRequest(isNull: true);

            //Act
            var actual = await _testing.Update(request);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task GIVEN_ThereIsAnUpdateRequest_WHEN_UpdatingACategory_THEN_ShouldReturnOkResultAndCallServiceOnce()
        {
            //Arrange
            var request = GetCategoryUpdateRequest();

            //Act
            var actual = await _testing.Update(request);

            //Assert
            Assert.IsType<OkResult>(actual);

            _mockCategoryService
                .Verify(service =>
                    service.UpdateAsync(
                        request.Id,
                        request.Name,
                        request.Order,
                        request.Image),
                    Times.Once);
        }

        [Fact]
        public async Task GIVEN_ThereIsAnUpdateRequestNotInTheDatabase_WHEN_UpdatingACategory_THEN_ShoudReturnNotFound()
        {
            //Arrange
            var request = GetCategoryUpdateRequest();
            _mockCategoryService
                .Setup(service =>
                    service.UpdateAsync(
                                It.IsAny<int>(),
                                It.IsAny<string>(),
                                It.IsAny<int>(),
                                It.IsAny<byte[]>()))
                .Throws(new ObjectNotFoundException(""));

            //Act
            var actual = await _testing.Update(request);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actual);
        }

        [Fact]
        public async Task GIVEN_ThereIsAnUpdateRequestAndExceptionInService_WHEN_UpdatingACategory_THEN_ShouldThrowAnException()
        {
            //Arrange
            var request = GetCategoryUpdateRequest();
            _mockCategoryService
                .Setup(service =>
                    service.UpdateAsync(
                                It.IsAny<int>(),
                                It.IsAny<string>(),
                                It.IsAny<int>(),
                                It.IsAny<byte[]>()))
                .Throws<Exception>();

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _testing.Update(request));
        }

        [Fact]
        public async Task GIVEN_ThereIsAnEmptyInsertRequest_THEN_ShouldGetBadRequest()
        {
            CategoryInsertRequest insertRequest = null;

            //Act
            var actual = await _testing.Insert(insertRequest);

            //Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async Task GIVEN_ThereIsAnInsertRequest_WHEN_InsertingACategory_THEN_ShouldReturnOkResultAndCallServiceOnce()
        {
            //Arrange
            var actual = await _testing.Insert(_request);

            //Assert
            _mockCategoryService
                .Verify(service =>
                        service.InsertAsync(
                            _request.Name,
                            _request.Order,
                            _byteImage)
                    );
            Assert.IsType<OkResult>(actual);
        }

    }
}
