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
            _logger.LogInformation($"{GetCategories(versionNumber)} is started with version number = {versionNumber}");
            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategories(versionNumber);
            if (categoryResult == null ||
                (!categoryResult.IsUpdated && categoryResult.Categories == null))
            {
                return NoContent();
            }
            _logger.LogInformation($"{GetCategories(versionNumber)} is finished successfully");
            return ToCategoryResponse(categoryResult);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CategoryInsertRequest insertRequest)
        {
            _logger.LogInformation($"{Insert(insertRequest)} is started with insert request = {insertRequest}");
            if (insertRequest == null)
            {
                return BadRequest();
            }

            var byteImage = Convert.FromBase64String(insertRequest.Image);

            await _categoryService.InsertAsync(
                insertRequest.Name,
                insertRequest.Order,
                byteImage);

            _logger.LogInformation($"{Insert(insertRequest)} is finished successfully");
            return Ok();
        }

        [HttpPost, Route("update/{id}")]
        public async Task<IActionResult> Update(CategoryUpdateRequest updateRequest)
        {
            _logger.LogInformation($"{Update(updateRequest)} is started with update request = {updateRequest}");
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
            _logger.LogInformation($"{Update(updateRequest)} is finished successfully");
        }

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"{Delete(id)} is started with  id = {id}");
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
            _logger.LogInformation($"{Delete(id)} is finished successfully");
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
};

