using System.Collections.Generic;
using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Order { get; set; }

        public Image Image { get; set; }

        public ICollection<CategoryDetailsEntity> CategoryDetails { get; set; }
    }
}
