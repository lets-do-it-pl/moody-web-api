using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsInsertRequest
    {
        [Required]
        public string Image { get; set; }
    }
}
