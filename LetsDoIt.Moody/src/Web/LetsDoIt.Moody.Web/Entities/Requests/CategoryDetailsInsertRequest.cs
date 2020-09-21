using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryDetailsInsertRequest
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int Id { get; set; }

        public int Order { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
