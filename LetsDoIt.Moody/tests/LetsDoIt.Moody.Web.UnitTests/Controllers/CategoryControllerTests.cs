//using System;
//using Moq;
//using Xunit;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace LetsDoIt.Moody.Web.UnitTests.Controllers
//{
//    using Application.Category;
//    using Application.CustomExceptions;
//    using Web.Controllers;
//    using Entities.Requests;

//    public class CategoryControllerTests
//    {
//        private readonly CategoryController _testing;

//        private readonly byte[] _byteImage;
//        private readonly int _categoryId;
//        private readonly CategoryDetailsInsertRequest _insertRequest;
//        private readonly CategoryDetailsUpdateRequest _updateRequest;
//        private readonly CategoryDetailsUpdateRequest _updateRequestWithoutId;
//        private readonly CategoryDetailsUpdateRequest _updateRequstWithoutImage;
//        private readonly CategoryInsertRequest _request;
//        private readonly byte[] image = { 12, 45, 65, 34, 78, 89 };
//        private readonly Mock<ICategoryService> _mockCategoryService;
//        private readonly Mock<ILogger<CategoryController>> _mockLogger;

//        #region SetUp & Helpers

//        public CategoryControllerTests()
//        {
//            _mockLogger = new Mock<ILogger<CategoryController>>();

//            _mockCategoryService = new Mock<ICategoryService>();

//            _testing = new CategoryController(_mockCategoryService.Object, _mockLogger.Object);

//            _updateRequstWithoutImage = new CategoryDetailsUpdateRequest
//            {
//                Order = 3
//            };

//            _updateRequestWithoutId = new CategoryDetailsUpdateRequest
//            {
//                Order = 5,
//                Image = image
//            };
//            _updateRequest = new CategoryDetailsUpdateRequest
//            {
//                Image = image,
//                Order = 4
//            };
//            _insertRequest = new CategoryDetailsInsertRequest
//            {
//                Image = "cGxlYXN1cmUu",
//                Order = 4
//            };
//            _request = new CategoryInsertRequest
//            {
//                Name = "adsfasdf",
//                Order = 5,
//                Image = "USrCELxGejBZI4W/Llsvmw==\r\n"
//            };
//            _byteImage = Convert.FromBase64String(_request.Image);
//            _categoryId = 5;
//        }

//        private CategoryUpdateRequest GetCategoryUpdateRequest(
//            byte[] image,
//            string name = "name",
//            int order = 1,
//            bool isNull = false)
//        {
//            if (isNull)
//            {
//                return null;
//            }

//            return new CategoryUpdateRequest
//            {
//                Name = name,
//                Image = image,
//                Order = order
//            };
//        }

//        private CategoryUpdateRequest GetCategoryUpdateRequest(
//            string name = "name",
//            int order = 1,
//            bool isNull = false)
//        {
//            return GetCategoryUpdateRequest(new byte[] { 1 }, name, order, isNull);
//        }

//        #endregion

//        [Fact]
//        public async Task GIVEN_ThereIsNoUpdateRequest_WHEN_UpdatingACategory_THEN_ShouldGetBadRequest()
//        {
//            //Arrange
//            var request = GetCategoryUpdateRequest(isNull: true);

//            //Act
//            var actual = await _testing.Update(default, request);

//            //Assert
//            Assert.IsType<BadRequestResult>(actual);
//        }

//        [Fact]
//        public async Task GIVEN_ThereIsAnUpdateRequest_WHEN_UpdatingACategory_THEN_ShouldReturnOkResultAndCallServiceOnce()
//        {
//            //Arrange
//            var categoryId = 1;
//            var request = GetCategoryUpdateRequest();

//            //Act
//            var actual = await _testing.Update(categoryId, request);

//            //Assert
//            Assert.IsType<OkResult>(actual);

//            _mockCategoryService
//                .Verify(service =>
//                    service.UpdateAsync(
//                        categoryId,
//                        request.Name,
//                        request.Order,
//                        request.Image),
//                    Times.Once);
//        }

//        [Fact]
//        public async Task GIVEN_ThereIsAnUpdateRequestNotInTheDatabase_WHEN_UpdatingACategory_THEN_ShoudReturnNotFound()
//        {
//            //Arrange
//            var request = GetCategoryUpdateRequest();
//            _mockCategoryService
//                .Setup(service =>
//                    service.UpdateAsync(
//                                It.IsAny<int>(),
//                                It.IsAny<string>(),
//                                It.IsAny<int>(),
//                                It.IsAny<byte[]>()))
//                .Throws(new ObjectNotFoundException(""));

//            //Act
//            var actual = await _testing.Update(default, request);

//            //Assert
//            Assert.IsType<NotFoundObjectResult>(actual);
//        }

//        [Fact]
//        public async Task GIVEN_ThereIsAnUpdateRequestAndExceptionInService_WHEN_UpdatingACategory_THEN_ShouldThrowAnException()
//        {
//            //Arrange
//            var request = GetCategoryUpdateRequest();
//            _mockCategoryService
//                .Setup(service =>
//                    service.UpdateAsync(
//                                It.IsAny<int>(),
//                                It.IsAny<string>(),
//                                It.IsAny<int>(),
//                                It.IsAny<byte[]>()))
//                .Throws<Exception>();

//            //Act & Assert
//            await Assert.ThrowsAsync<Exception>(() => _testing.Update(default, request));
//        }

//        [Fact]
//        public async Task GIVEN_ThereIsAnEmptyInsertRequest_THEN_ShouldGetBadRequest()
//        {
//            CategoryInsertRequest insertRequest = null;

//            //Act
//            var actual = await _testing.Insert(insertRequest);

//            //Assert
//            Assert.IsType<BadRequestResult>(actual);
//        }

//        [Fact]
//        public async Task GIVEN_ThereIsAnInsertRequest_WHEN_InsertingACategory_THEN_ShouldReturnOkResultAndCallServiceOnce()
//        {
//            //Arrange
//            var actual = await _testing.Insert(_request);

//            //Assert
//            _mockCategoryService
//                .Verify(service =>
//                        service.InsertAsync(
//                            _request.Name,
//                            _request.Order,
//                            _byteImage)
//                    );
//            Assert.IsType<OkResult>(actual);
//        }

//        [Fact]
//        public async Task InsertCategoryDetails_NullInsertRequest_ShouldReturnBadRequest()
//        {
//            CategoryDetailsInsertRequest insertRequest = null;

//            var categoryId  = 5;

//            var actual = await _testing.InsertCategoryDetails(categoryId,insertRequest);

//            Assert.IsType<BadRequestResult>(actual);
//        }

//        [Fact]
//        public async Task InsertCategoryDetails_ThereIsAnInsertRequest_ShouldReturnOk()
//        {
//            var actual = await _testing.InsertCategoryDetails(default,_insertRequest);

//            _mockCategoryService
//                .Verify(service =>
//                        service.InsertCategoryDetailsAsync(
//                            default,
//                            _insertRequest.Order,
//                            _insertRequest.Image)
//                    , Times.Once);
//            Assert.IsType<OkResult>(actual);
//        }

//        [Fact]
//        public async Task DeleteCategoryDetails_WithoutId_ShoulThrowObjectNotFoundException()
//        {
//            var id = 3;

//            _mockCategoryService
//                .Setup(service =>
//                    service.DeleteCategoryDetailsAsync(
//                                It.IsAny<int>()))
//                .Throws(new ObjectNotFoundException(""));

//            var actual = await _testing.DeleteCategoryDetails(id);

//            Assert.IsType<NotFoundObjectResult>(actual);

//        }

//        [Fact]
//        public async Task DeleteCategoryDetails_IdExists_ShoulReturnOk()
//        {
//            var id = 3;

//            _mockCategoryService
//                .Setup(service =>
//                    service.DeleteCategoryDetailsAsync(
//                                It.IsAny<int>()));

//            var actual = await _testing.DeleteCategoryDetails(id);
//            Assert.IsType<OkResult>(actual);

//        }

//        [Fact]
//        public async Task UpdateCategoryDetails_NullUpdateRequest_ShouldReturnBadRequest()
//        {
//            CategoryDetailsUpdateRequest request = null;

//            var actual = await _testing.UpdateCategoryDetails(default,request);

//            Assert.IsType<BadRequestResult>(actual);
//        }

//        [Fact]
//        public async Task UpdateCategoryDetails_UpdateRequest_ShouldReturnOk()
//        {
//            var actual = await _testing.UpdateCategoryDetails(default,_updateRequest);

//            _mockCategoryService
//                .Verify(service =>
//                        service.UpdateCategoryDetailsAsync(
//                            default,
//                            _updateRequest.Order,
//                            _updateRequest.Image)
//                    , Times.Once);

//            Assert.IsType<OkResult>(actual);
//        }

//        [Fact]
//        public async Task UpdateCategoryDetails_UpdateRequestWithoutId_ShouldThrowObjectNotFoundException()
//        {
//            _mockCategoryService
//                .Setup(c =>
//                        c.UpdateCategoryDetailsAsync(
//                            It.IsAny<int>(),
//                            _updateRequestWithoutId.Order,
//                             _updateRequestWithoutId.Image)
//                    ).Throws(new ObjectNotFoundException(""));

//            var actual = await _testing.UpdateCategoryDetails(default,_updateRequestWithoutId);

//            Assert.IsType<NotFoundObjectResult>(actual);
//        }

//        [Fact]
//        public async Task UpdateCategoryDetails_UpdateRequestWithoutImage_ShouldThrowAnException()
//        {
//            _mockCategoryService
//                .Setup(service =>
//                    service.UpdateCategoryDetailsAsync(
//                                default,
//                                _updateRequstWithoutImage.Order,
//                                It.IsAny<byte[]>()))
//                .Throws<Exception>();

//            await Assert.ThrowsAsync<Exception>(() => _testing.UpdateCategoryDetails(default,_updateRequstWithoutImage));
//        }
//    }
//}
