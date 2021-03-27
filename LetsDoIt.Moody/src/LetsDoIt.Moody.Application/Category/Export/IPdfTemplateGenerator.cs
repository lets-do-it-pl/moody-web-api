using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using LetsDoIt.Moody.Application.Data;
    public interface IPdfTemplateGenerator
    {
         string GetHTMLString(IEnumerable<CategoryUserReturnResult> categories);
    }
}