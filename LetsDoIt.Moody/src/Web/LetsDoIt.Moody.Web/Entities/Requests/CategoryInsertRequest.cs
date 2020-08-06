using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryInsertRequest
    {
        [Required]
        public string Name { get; set; }

        public int Order { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
