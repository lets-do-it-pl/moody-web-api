using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using NLog;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.CustomExceptions;
    using Entities.Requests;
    using Entities.Responses;

    [ApiController]
    [Route("api/categories")]
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

        [HttpGet, Route("{versionNumber?}")]
        public async Task<ActionResult<CategoryResponse>> GetCategories(string versionNumber = null)
        {
            _logger.LogInformation("START Get all Categories");
            _logger.LogInformation("versionNumber : {0}",(versionNumber));

            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategories(versionNumber);
            if (categoryResult == null ||
                (!categoryResult.IsUpdated && categoryResult.Categories == null))
            {
                _logger.LogInformation("categoryResult or categoryResult.IsUpdate and " +
                                       "categoryResult.Categories with versionNumber : {0} does not exits",(versionNumber));
                return NoContent();
            }
            _logger.LogInformation("End Get all Categories");
            return ToCategoryResponse(categoryResult);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CategoryInsertRequest insertRequest)
        {
            _logger.LogInformation("START Insert");
            _logger.LogInformation("INPUT object Name : {0}, Order : {1}, Image : {2} ",(insertRequest.Name),
                (insertRequest.Order),(insertRequest.Image) );

            if (insertRequest == null)
            {
                _logger.LogInformation("insertRequest : {0} does not exist",insertRequest);
                return BadRequest();
            }

            var byteImage = Convert.FromBase64String(insertRequest.Image);

            await _categoryService.InsertAsync(
                insertRequest.Name,
                insertRequest.Order,
                byteImage);

            _logger.LogInformation("End Insert");
            return Ok();
        }

        [HttpPost, Route("update/{id}")]
        public async Task<IActionResult> Update(CategoryUpdateRequest updateRequest)
        {
            _logger.LogInformation("START Update");
            _logger.LogInformation("INPUT object {0}", (updateRequest));

            if (updateRequest == null)  
            {
                _logger.LogInformation("updateRequest : {0} does not exist", updateRequest);
                return BadRequest();
            }

            try
            {
                await _categoryService.UpdateAsync(
                    updateRequest.Id,
                    updateRequest.Name,
                    updateRequest.Order,
                    updateRequest.Image);

                _logger.LogInformation("END Update Order");
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
            _logger.LogInformation("START Delete");
            _logger.LogInformation("INPUT ID {0}", id);
            _logger.LogInformation("END  Delete");
            try
            {
                await _categoryService.DeleteAsync(id);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogError("Delete error {0}", id);
                return NotFound(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private CategoryResponse ToCategoryResponse(CategoryGetResult categoryResult)
        {

            _logger.LogInformation("START to Category Response");
            _logger.LogInformation("INPUT object {0}", categoryResult);

            var result = new CategoryResponse
                        {
                            IsUpdated = categoryResult.IsUpdated,
                            VersionNumber = categoryResult.VersionNumber                            
                        };

            if(categoryResult.Categories != null)
            {

                _logger.LogInformation("categoryResult.Categories with categoryResult : {0}", categoryResult);

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
            _logger.LogInformation("END to Category Response");
            return result;
        }
    }
}