using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsUpdateRequest
    { 
        [Required]
        public int Id { get; set; }

        public int Order { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
