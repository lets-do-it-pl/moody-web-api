using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Web.Entities.Requests.Category
{
    public class CategoryDetailsUpdateRequest
    { 
        [Required]
        public Image Image { get; set; }
    }
}
