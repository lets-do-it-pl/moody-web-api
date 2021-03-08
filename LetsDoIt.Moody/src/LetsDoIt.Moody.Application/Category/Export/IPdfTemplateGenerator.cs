using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using LetsDoIt.Moody.Application.Data;
    using Persistence.Entities;
    public interface IPdfTemplateGenerator
    {
         Task<string> GetHTMLStringAsync(ICollection<Category> categories, IEnumerable<CategoryUserReturnResult> users);
    }
}
