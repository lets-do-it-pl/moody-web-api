using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.Constants;
    using Application.CustomExceptions;
    using Entities;
    using Entities.Requests;
    using Entities.Responses;
    using LetsDoIt.Moody.Web.Cache;
    using Persistence.Entities;

    [ApiController]
    [Route("api/category")]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet, Route("/list-detail/{versionNumber?}")]
        [Authorize(Roles = RoleConstants.ClientRole)]
        [Cached(600)]
        public async Task<ActionResult<CategoryResponse>> GetCategories(string versionNumber = null)
        {
            
            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategoriesWithDetails(versionNumber);
            if (categoryResult == null ||
                (!categoryResult.IsUpdated && categoryResult.Categories == null))
            {
                return NoContent();
            }

            return ToCategoryResponse(categoryResult);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CategoryInsertRequest insertRequest)
        {

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

            return Ok();
        }

        [HttpPost]
        [Route("{categoryId}/detail")]
        public async Task<IActionResult> InsertCategoryDetails(int categoryId, [FromBody] CategoryDetailsInsertRequest insertRequest)
        {

            if (insertRequest == null)
            {
                return BadRequest();
            }

            await _categoryService.InsertCategoryDetailsAsync(
                categoryId,
                insertRequest.Order,
                insertRequest.Image,
                GetUserInfo().UserId);

            return Ok();
        }

        [HttpPut, Route("{categoryId}")]
        public async Task<IActionResult> Update(int categoryId, CategoryUpdateRequest updateRequest)
        {

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

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryId);
            }
        }

        [HttpPut, Route("{categoryId}/detail/{categoryDetailsId}")]
        public async Task<IActionResult> UpdateCategoryDetails(int categoryDetailsId, CategoryDetailsUpdateRequest updateRequest)
        {

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

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryDetailsId);
            }
        }

        [HttpDelete, Route("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            try
            {
                await _categoryService.DeleteAsync(
                    categoryId,
                    GetUserInfo().UserId);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryId);
            }
        }

        [HttpDelete, Route("{categoryId}/detail/{categoryDetailsId}")]
        public async Task<IActionResult> DeleteCategoryDetails(int categoryDetailsId)
        {

            try
            {
                await _categoryService.DeleteCategoryDetailsAsync(
                    categoryDetailsId,
                    GetUserInfo().UserId);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
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

        private static CategoryEntity ToCategoryEntity(Category c)
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

        private static CategoryDetailsEntity ToCategoryDetailsEntity(CategoryDetail c) => new CategoryDetailsEntity
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