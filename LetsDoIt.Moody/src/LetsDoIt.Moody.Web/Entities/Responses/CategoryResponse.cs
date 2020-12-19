using System.Collections.Generic;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Order { get; set; }

        public byte[] Image { get; set; }

        public ICollection<CategoryDetailsResponse> CategoryDetails { get; set; }
    }
}
