using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Web.Entities.Requests.Category
{
    public class CategoryInsertRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Image Image { get; set; }

        public string Description { get; set; }
    }
}
