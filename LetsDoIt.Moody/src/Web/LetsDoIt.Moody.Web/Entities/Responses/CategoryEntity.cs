using System.Collections.Generic;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public byte[] Image { get; set; }

        public ICollection<CategoryDetailsEntity> CategoryDetails { get; set; }
    }
}
