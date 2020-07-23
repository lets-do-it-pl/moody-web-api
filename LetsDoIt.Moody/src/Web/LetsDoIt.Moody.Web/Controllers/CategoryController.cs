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
        public void Insert( string name, int order, byte[] image)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null!");
            }

            if(image == null)
            {
                throw new ArgumentException("Image cannot be null!");
            }

            _categoryService.Insert( name, order, image);
        }
    }
}