using System;
using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryInsertRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        public string Description { get; set; }

    }
}
