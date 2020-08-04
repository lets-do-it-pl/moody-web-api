using Microsoft.AspNetCore.Mvc;
using System;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Category;
    using System.Threading.Tasks;

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
        public async Task Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
        }
        
        [HttpPost]
        public async Task Insert(string name, int order, byte[] image)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name can not be null");
            }

            if (image == null)
            {
                throw new ArgumentException("Image cannot be null!");
            }

            await _categoryService.InsertAsync(name, order, image);

        }
    }
}