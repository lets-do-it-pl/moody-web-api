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


        
    }
}