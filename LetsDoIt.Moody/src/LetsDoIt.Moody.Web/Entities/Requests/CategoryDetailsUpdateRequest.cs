using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsUpdateRequest
    { 
        public decimal Order { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
