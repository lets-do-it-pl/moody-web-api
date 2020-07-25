using Microsoft.AspNetCore.Mvc;
using System;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application;

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
        [Route("{id}")]
        public async Task Update(int id, string name, int order, byte[] image)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null!");
            }

            if(image == null)
            {
                throw new ArgumentException("Image cannot be null!");
            }

            await _categoryService.UpdateAsync(id, name, order, image);
        }
    }
}