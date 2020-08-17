using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class CategoryUpdateRequest
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Order { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
