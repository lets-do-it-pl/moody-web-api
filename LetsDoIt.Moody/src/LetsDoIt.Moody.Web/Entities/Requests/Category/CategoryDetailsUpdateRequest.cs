using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsUpdateRequest
    { 
        [Required]
        public byte[] Image { get; set; }
    }
}
