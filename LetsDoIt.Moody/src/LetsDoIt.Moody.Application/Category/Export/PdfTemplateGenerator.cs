using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using LetsDoIt.Moody.Application.Data;
    using Persistence.Entities;
    using System.Linq;
    using System.Threading.Tasks;

    public class PdfTemplateGenerator : IPdfTemplateGenerator
    {
        public async Task<string> GetHTMLStringAsync(ICollection<Category> categories, IEnumerable<CategoryUserReturnResult> users)
        {
            var sb = new StringBuilder();

            sb.Append(@"
                 <html>
                    <thead>
                        <table border='5' width= '80%' cellpanding= '4' cellspacing= '3' align='center'>
                            <tr>
                                <th>Id</th>
                                <th>Name</th>
                                <th>Image Count</th>
                                <th>Create Date</th>
                                <th>Created By</th>
                                <th>Modified Date</th>
                                <th>Modified By</th>
                            </tr>");

            var index = 1;
            foreach (var category in categories)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>   
                                  </tr>", index, category.Name,
                                   category.CategoryDetails.Select(c => !c.IsDeleted).Count(), category.CreatedDate,
                                  users.FirstOrDefault(u => u.Id == category.Id).CreatedBy, users.FirstOrDefault(u => u.Id == category.Id).ModifiedBy, category.ModifiedDate, category.ModifiedBy);
            }
            sb.Append(@"
                        </table>
                    </thead>
                </html>");

            return sb.ToString();
        }
    }
}


