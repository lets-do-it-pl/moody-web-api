using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using Application.CustomExceptions;
    using Entities.Requests;    

    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]CategoryInsertRequest insertRequest)
        {
            if(insertRequest == null)
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
        [Route("update/{id}")]
        public async Task<IActionResult> Update(CategoryUpdateRequest updateRequest)
        {
            if(updateRequest == null)
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


        [HttpDelete]
        [Route("{id}")]
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
    }
}