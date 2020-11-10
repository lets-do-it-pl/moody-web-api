using System.Collections.Generic;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoriesResponse
    {
        public IEnumerable<CategoryEntity> Categories { get; set; }
    }
}
