using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.Constants;
    using Application.CustomExceptions;
    using Entities;
    using Entities.Requests;
    using Entities.Responses;
    using Persistence.Entities;

    [ApiController]
    [Route("api/category")]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet, Route("/list-detail/{versionNumber?}")]
        [Authorize(Roles = RoleConstants.ClientRole)]
        public async Task<ActionResult<CategoryResponse>> GetCategories(string versionNumber = null)
        {
            _logger.LogInformation($"{nameof(GetCategories)} is started with version number = {versionNumber}");

            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategoriesWithDetails(versionNumber);
            if (categoryResult == null ||
                (!categoryResult.IsUpdated && categoryResult.Categories == null))
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetCategories)} is finished successfully");

            return ToCategoryResponse(categoryResult);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CategoryInsertRequest insertRequest)
        {
            _logger.LogInformation($"{nameof(Insert)} is started with insert request = {insertRequest}");

            if (insertRequest == null)
            {
                return BadRequest();
            }

            var byteImage = Convert.FromBase64String(insertRequest.Image);

            await _categoryService.InsertAsync(
                insertRequest.Name,
                insertRequest.Order,
                byteImage,
                GetUserInfo().UserId);

            _logger.LogInformation($"{nameof(Insert)} is finished successfully");

            return Ok();
        }

        [HttpPost]
        [Route("{categoryId}/detail")]
        public async Task<IActionResult> InsertCategoryDetails(int categoryId, [FromBody] CategoryDetailsInsertRequest insertRequest)
        {
            _logger.LogInformation(
                $"{nameof(InsertCategoryDetails)} is started with " +
                $"category Id = {categoryId}; " +
                $"insertRequest = {JsonConvert.SerializeObject(insertRequest)}");

            if (insertRequest == null)
            {
                return BadRequest();
            }

            await _categoryService.InsertCategoryDetailsAsync(
                categoryId,
                insertRequest.Order,
                insertRequest.Image,
                GetUserInfo().UserId);

            _logger.LogInformation($"{nameof(InsertCategoryDetails)} is finished successfully");

            return Ok();
        }

        [HttpPut, Route("{categoryId}")]
        public async Task<IActionResult> Update(int categoryId, CategoryUpdateRequest updateRequest)
        {
            _logger.LogInformation(
                $"{nameof(Update)} is started with " +
                $"update request = {JsonConvert.SerializeObject(updateRequest)}");

            if (updateRequest == null)
            {
                return BadRequest();
            }

            try
            {
                await _categoryService.UpdateAsync(
                    categoryId,
                    updateRequest.Name,
                    updateRequest.Order,
                    updateRequest.Image,
                    GetUserInfo().UserId);

                _logger.LogInformation($"{nameof(Update)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(Update)} is finished with Not Found!");

                return NotFound(categoryId);
            }
        }

        [HttpPut, Route("{categoryId}/detail/{categoryDetailsId}")]
        public async Task<IActionResult> UpdateCategoryDetails(int categoryDetailsId, CategoryDetailsUpdateRequest updateRequest)
        {
            _logger.LogInformation(
                $"{nameof(UpdateCategoryDetails)} is started with " +
                $"category detailsId = {categoryDetailsId} " +
                $"update request = {JsonConvert.SerializeObject(updateRequest)}");

            if (updateRequest == null)
            {
                return BadRequest();
            }

            try
            {
                await _categoryService.UpdateCategoryDetailsAsync(
                    categoryDetailsId,
                    updateRequest.Order,
                    updateRequest.Image,
                    GetUserInfo().UserId);

                _logger.LogInformation($"{nameof(UpdateCategoryDetails)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(UpdateCategoryDetails)} is finished with Not Found!");

                return NotFound(categoryDetailsId);
            }
        }

        [HttpDelete, Route("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            _logger.LogInformation(
                $"{nameof(Delete)} is started with " +
                $"id = {categoryId}");

            try
            {
                await _categoryService.DeleteAsync(
                    categoryId,
                    GetUserInfo().UserId);

                _logger.LogInformation($"{nameof(Delete)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(Delete)} is finished with Not Found.");

                return NotFound(categoryId);
            }
        }

        [HttpDelete, Route("{categoryId}/detail/{categoryDetailsId}")]
        public async Task<IActionResult> DeleteCategoryDetails(int categoryDetailsId)
        {
            _logger.LogInformation(
                $"{nameof(DeleteCategoryDetails)} is started with " +
                $"category detailsId = {categoryDetailsId}");

            try
            {
                await _categoryService.DeleteCategoryDetailsAsync(
                    categoryDetailsId,
                    GetUserInfo().UserId);

                _logger.LogInformation($"{nameof(DeleteCategoryDetails)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(DeleteCategoryDetails)} is finished with Not Found");

                return NotFound(categoryDetailsId);
            }
        }

        private static CategoryResponse ToCategoryResponse(CategoryGetResult categoryResult)
        {
            var result = new CategoryResponse
            {
                IsUpdated = categoryResult.IsUpdated,
                VersionNumber = categoryResult.VersionNumber
            };

            if (categoryResult.Categories != null)
            {
                result.Categories = categoryResult
                    .Categories
                    .Select(ToCategoryEntity);
            }

            return result;
        }

        private static CategoryEntity ToCategoryEntity(Persistence.Entities.Category c)
        {
            var result = new CategoryEntity
            {
                Id = c.Id,
                Name = c.Name,
                Order = c.Order,
                Image = c.Image
            };

            if (c.CategoryDetails != null)
            {
                result.CategoryDetails = c.CategoryDetails.Select(ToCategoryDetailsEntity).ToList();
            }

            return result;
        }

        private static CategoryDetailsEntity ToCategoryDetailsEntity(Persistence.Entities.CategoryDetail c) => new CategoryDetailsEntity
        {
            Id = c.Id,
            Image = c.Image,
            Order = c.Order
        };

        private UserInfo GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return new UserInfo(userId, fullName);
        }
    }
}