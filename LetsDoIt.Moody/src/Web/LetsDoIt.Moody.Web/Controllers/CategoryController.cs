using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace LetsDoIt.Moody.Web.Controllers
{
    using System.Collections.Generic;
    using Application.Category;
    using Application.CustomExceptions;
    using Domain;
    using Entities.Requests;
    using Entities.Responses;    
    using Filters;

    [ApiController]
    [EnableCors("Policy")]
    [Route("api/categories")]
    //[Authorization]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public IEnumerable<Category> Categories { get; set; }

        public CategoryController(ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet, Route("{versionNumber?}")]
        public async Task<ActionResult<CategoryResponse>> GetCategoriesWithDetails(string versionNumber = null)
        {
            _logger.LogInformation($"{nameof(GetCategoriesWithDetails)} is started with version number = {versionNumber}");

            versionNumber = !string.IsNullOrWhiteSpace(versionNumber) ? versionNumber.Trim() : string.Empty;

            var categoryResult = await _categoryService.GetCategoriesWithDetails(versionNumber);
            if (categoryResult == null ||
                (!categoryResult.IsUpdated && categoryResult.Categories == null))
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetCategoriesWithDetails)} is finished successfully");

            return ToCategoryResponseWithDetails(categoryResult);
        }

        [HttpGet, Route("/list")]
        public async Task<ActionResult<CategoriesResponse>> GetCategories()
        {
            _logger.LogInformation($"{nameof(GetCategories)} is started.");

            var categoryResult = await _categoryService.GetCategories();
            if (categoryResult == null)
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetCategories)} is finished successfully");

            return ToCategoryResponse(categoryResult);
        }

        [HttpGet, Route("/{categoryId}/details")]
        public async Task<IEnumerable<CategoryDetailsEntity>> GetCategoryDetails(int categoryId)
        {
            _logger.LogInformation($"{nameof(GetCategoryDetails)} is started with the category Id = {categoryId}.");

            var categoryResult = await _categoryService.GetCategoryDetails(categoryId);
            if (categoryResult == null)
            {
                return null;
            }

            _logger.LogInformation($"{nameof(GetCategoryDetails)} is finished successfully");

            return categoryResult.Select(c => new CategoryDetailsEntity
                                                    {
                                                      Id = c.Id,
                                                      Order = c.Order,
                                                      Image = c.Image
                                                        });
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
                byteImage);

            _logger.LogInformation($"{nameof(Insert)} is finished successfully");

            return Ok();
        }

        [HttpPost]
        [Route("{categoryId}/details")]
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
                insertRequest.Image);

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
                    updateRequest.Image);

                _logger.LogInformation($"{nameof(Update)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(Update)} is finished with Not Found!");

                return NotFound(categoryId);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpPut, Route("{categoryId}/details/{categoryDetailsId}")]
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
                    updateRequest.Image);

                _logger.LogInformation($"{nameof(UpdateCategoryDetails)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(UpdateCategoryDetails)} is finished with Not Found!");

                return NotFound(categoryDetailsId);
            }
            catch (Exception)
            {
                throw;
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
                await _categoryService.DeleteAsync(categoryId);

                _logger.LogInformation($"{nameof(Delete)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(Delete)} is finished with Not Found.");

                return NotFound(categoryId);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpDelete, Route("{categoryId}/details/{categoryDetailsId}")]
        public async Task<IActionResult> DeleteCategoryDetails(int categoryDetailsId)
        {
            _logger.LogInformation(
                $"{nameof(DeleteCategoryDetails)} is started with " +
                $"category detailsId = {categoryDetailsId}");

            try
            {
                await _categoryService.DeleteCategoryDetailsAsync(categoryDetailsId);

                _logger.LogInformation($"{nameof(DeleteCategoryDetails)} is finished successfully");

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                _logger.LogInformation($"{nameof(DeleteCategoryDetails)} is finished with Not Found");

                return NotFound(categoryDetailsId);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private CategoryResponse ToCategoryResponseWithDetails(CategoryGetResult categoryResult)
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

            if (c.CategoryDetails != null)
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

        private CategoriesResponse ToCategoryResponse(CategoryGetResult categoryResult)
        {
            var result = new CategoriesResponse();

            if (categoryResult.Categories != null)
            {
                result.Categories = categoryResult
                                        .Categories
                                        .Select(c =>
                                             new Entities.Responses.CategoryEntity
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