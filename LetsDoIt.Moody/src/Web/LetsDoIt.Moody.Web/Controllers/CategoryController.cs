using System;
using Microsoft.AspNetCore.Mvc;
using Dawn;


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

        [HttpGet]
        [Route("{versionNumber}")]
        public void getCategory(string versionNumber)
        {
            var _versionNumber = Guard.Argument(versionNumber, nameof(versionNumber)).NotNull().NotEmpty();

            _categoryService.getCategory(_versionNumber);
        }
    }
}
