using System.Collections.Generic;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class VersionedCategoryWithDetailsResponse
    {
        public string VersionNumber { get; set; }

        public bool IsUpdated { get; set; }

        public IEnumerable<CategoryResponse> Categories { get; set; }
    }
}
