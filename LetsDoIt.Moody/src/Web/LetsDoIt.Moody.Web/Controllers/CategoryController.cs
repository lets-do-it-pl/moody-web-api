﻿using Microsoft.AspNetCore.Mvc;
using System;
using LetsDoIt.Moody.Application.Services;
using LetsDoIt.Moody.Application.Services.Abstract;

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
        public void Update(int id, string name, int order, byte[] image)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null!");
            }

            if(image == null)
            {
                throw new ArgumentException("Image cannot be null!");
            }

            _categoryService.Update(id, name, order, image);
        }
    }
}