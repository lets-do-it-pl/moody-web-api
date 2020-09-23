using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsUpdateRequest
    { 
        public int Order { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
