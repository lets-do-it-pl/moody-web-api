using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Web.Filters;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.CustomExceptions;
    using Entities.Requests;
    using Entities.Responses;

    [ApiController] 
    [Route("api/categories")]
    [TokenAuthorizationAttritbute()]
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

        [HttpPost, Route("update/{id}")]
        public async Task<IActionResult> Update(CategoryUpdateRequest updateRequest)
        {
            if (updateRequest == null)  
            {
                return BadRequest();
            }

            try
            {
                await _categoryService.UpdateAsync(
                    updateRequest.Id,
                    updateRequest.Name,
                    updateRequest.Order,
                    updateRequest.Image);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound(updateRequest.Id);
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

        private CategoryResponse ToCategoryResponse(CategoryGetResult categoryResult)
        {
            var result = new CategoryResponse
                        {
                            IsUpdated = categoryResult.IsUpdated,
                            VersionNumber = categoryResult.VersionNumber                            
                        };

            if(categoryResult.Categories != null)
            {
                result.Categories = categoryResult
                                        .Categories
                                        .Select(c =>
                                             new CategoryEntity
                                             {
                                                 Id = c.Id,
                                                 Name = c.Name,
                                                 Order = c.Order,
                                                 Image = c.Image
                                             });
            }

            return result;
        }
    }
}