using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Category
{
    using Domain;

    public class CategoryGetResult
    {
        public string VersionNumber { get; set; }

        public bool IsUpdated { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}
