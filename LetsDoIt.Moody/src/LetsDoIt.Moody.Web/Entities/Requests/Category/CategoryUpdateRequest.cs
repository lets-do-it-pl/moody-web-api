﻿using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryUpdateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}