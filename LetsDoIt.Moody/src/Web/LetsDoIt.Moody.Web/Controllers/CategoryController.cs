using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.CustomExceptions;
    using Entities.Requests;
    using Entities.Responses;
    using LetsDoIt.Moody.Domain;

    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet, Route("{versionNumber?}")]
        public async Task<ActionResult<CategoryResponse>> GetCategories(string versionNumber = null)
        {

            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategories(versionNumber);
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
                byteImage);

            return Ok();
        }

        [HttpPost]
        [Route("/{categoryId}/details")]
        public async Task<IActionResult> InsertCategoryDetails(int categoryId, [FromBody] CategoryDetailsInsertRequest insertRequest)
        {
            if (insertRequest == null)
            {
                return BadRequest();
            }

            await _categoryService.InsertCategoryDetailsAsync(
                categoryId,                
                insertRequest.Order,
                insertRequest.Image);

            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateRequest updateRequest)
        {
            if (updateRequest == null)
            {
                return BadRequest();
            }

            try
            {
                await _categoryService.UpdateAsync(
                    id,
                    updateRequest.Name,
                    updateRequest.Order,
                    updateRequest.Image);
                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(id);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPut, Route("/{categoryId}/details/{categoryDetailsId}")]
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
                    updateRequest.Image);
                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryDetailsId);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete, Route("/{categoryId}/details/{categoryDetailsId}")]
        public async Task<IActionResult> DeleteCategoryDetails(int categoryDetailsId)
        {
            try
            {
                await _categoryService.DeleteCategoryDetailsAsync(categoryDetailsId);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(categoryDetailsId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private CategoryResponse ToCategoryResponse(CategoryGetResult categoryResult)
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
                    .Select(c => ToCategoryEntity(c));
            }

            return result;
        }

        private static Entities.Responses.CategoryEntity ToCategoryEntity(Category c)
        {
            var result = new Entities.Responses.CategoryEntity
            {
                Id = c.Id,
                Name = c.Name,
                Order = c.Order,
                Image = c.Image                
            };

            if(c.CategoryDetails != null)
            {
                result.CategoryDetails = c.CategoryDetails.Select(c => ToCategoryDetailsEntity(c)).ToList();
            }

            return result;
        }

        private static CategoryDetailsEntity ToCategoryDetailsEntity(CategoryDetails c) => new CategoryDetailsEntity
        {
            Id = c.Id,
            Image = c.Image,
            Order = c.Order
        };
    }
}

