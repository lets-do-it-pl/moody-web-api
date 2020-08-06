using LetsDoIt.Moody.Domain;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class CategoryResponse
    {
        public string VersionNumber { get; set; }

        public bool IsUpdated { get; set; }

        public IEnumerable<CategoryEntity> Categories { get; set; }
    }
}
