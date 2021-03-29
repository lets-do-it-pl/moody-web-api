using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Web.Entities.Requests.Category
{
    public class CategoryDetailsInsertRequest
    {
        [Required]
        public Image Image { get; set; }
    }
}
