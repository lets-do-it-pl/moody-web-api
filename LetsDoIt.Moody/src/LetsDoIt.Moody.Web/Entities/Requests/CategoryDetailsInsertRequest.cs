using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsInsertRequest
    {
        public decimal Order { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
