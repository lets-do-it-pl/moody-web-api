using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.CustomExceptions;
    using Entities;
    using Entities.Requests;
    using Entities.Responses;
    using Application.Constants;
    using Persistence.Entities;

    [ApiController]
    [Route("api/category")]
  //  [Authorize(Roles = RoleConstants.StandardRole)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet, Route("list-detail/{versionNumber?}")]
   //     [Authorize(Roles = RoleConstants.ClientRole)]
  //      [EnableCors("MobilePolicy")]
        public async Task<ActionResult<VersionedCategoryWithDetailsResponse>> GetVersionedCategoriesWithDetails(string versionNumber = null)
        {
            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategoriesWithDetailsAsync(versionNumber);
            if (categoryResult == null ||
                (!categoryResult.IsUpdated && categoryResult.Categories == null))
            {
                return NoContent();
            }

            return ToCategoryWithDetailsResponse(categoryResult);
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var categoryResult = await _categoryService.GetCategoriesAsync();
            if (categoryResult == null)
            {
                return NoContent();
            }

            var result = categoryResult
                            .OrderBy(c => c.Order)
                            .Select(ToCategoryResponse);

            return Ok(result);
        }

        [HttpGet, Route("{categoryId}/details")]
        public async Task<ActionResult<IEnumerable<CategoryDetailsResponse>>> GetCategoryDetails(int categoryId)
        {
            var categoryResult = await _categoryService.GetCategoryDetailsAsync(categoryId);
            if (categoryResult == null)
            {
                return NoContent();
            }

            var result = categoryResult
                            .OrderBy(cd => cd.Order)
                            .Select(ToCategoryDetailsResponse);

            return Ok(result);
        }

        [HttpGet]
        [Route("export/{type}")]
        public async Task<IActionResult> GetCategoryExport(string type)
        {
            if (type == null || type == " " || type == "")
            {
                return NoContent();
            }

            var categoryExportResult = _categoryService.GetCategoryExportAsync(type);

            return File(categoryExportResult.Result.Content, categoryExportResult.Result.ContentType, categoryExportResult.Result.FileName);
        }


        [HttpGet, Route("remove/cache")]
        public void RemoveCache()
        {
            _categoryService.RemoveCache();
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
                byteImage,
                GetUserInfo().UserId,
                insertRequest.Description
                );

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

            await _categoryService.InsertCategoryDetailAsync(
                categoryId,
                insertRequest.Image
                ,GetUserInfo().UserId
                );

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
                    updateRequest.Image,
                    GetUserInfo().UserId,
                    updateRequest.Description

                    );

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
                    updateRequest.Image
                    ,GetUserInfo().UserId
                    );

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryDetailsId);
            }
        }

        [HttpPut, Route("order/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateRequest updateRequest)
        {
            await _categoryService.UpdateOrderAsync(
                id,
                GetUserInfo().UserId,
                updateRequest.PreviousId,
                updateRequest.NextId
                );

            return Ok();
        }

        [HttpDelete, Route("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            try
            {
                await _categoryService.DeleteAsync(
                    categoryId
                    ,GetUserInfo().UserId
                    );

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
                    categoryDetailsId
                    ,GetUserInfo().UserId
                    );

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryDetailsId);
            }
        }
        
        private static VersionedCategoryWithDetailsResponse ToCategoryWithDetailsResponse(CategoryGetResult categoryResult)
        {
            var result = new VersionedCategoryWithDetailsResponse
            {
                IsUpdated = categoryResult.IsUpdated,
                VersionNumber = categoryResult.VersionNumber
            };

            if (categoryResult.Categories != null)
            {
                result.Categories = categoryResult
                    .Categories
                    .Select(ToCategoryResponse);
            }

            return result;
        }
                
        private static CategoryResponse ToCategoryResponse(Category c)
        {
            var result = new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Order = c.Order,
                Image = c.Image
            };
            return result;
        }

        private static CategoryDetailsResponse ToCategoryDetailsResponse(CategoryDetail c) => new CategoryDetailsResponse
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