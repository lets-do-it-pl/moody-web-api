using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Category
{
    using Domain;

    public class CategoryDetailsGetResult
    {
        public IEnumerable<CategoryDetails> CategoryDetails { get; set; }
    }
}
