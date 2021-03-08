using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using Data;

    public class PdfTemplateGenerator : IPdfTemplateGenerator
    {
        public Task<string> GetHTMLString(IEnumerable<CategoryUserReturnResult> categories)
        {
            var sb = new StringBuilder();

            sb.Append(@"
                 <html>
                    <thead>
                        <table border='5' width= '80%' cellpanding= '4' cellspacing= '3' align='center'>
                            <tr>  
                                <th>Category Name</th>
                                <th>Image Count</th>
                                <th>Create Date</th>
                                <th>Created By</th>
                                <th>Modified Date</th>
                                <th>Modified By</th>
                            </tr>");

            foreach (var category in categories)
            {
                var name = category.Name;
                var image = category.Image;
                var createdDate = category.CreatedDate.ToShortDateString();
                var createdby = category.CreatedBy;
                var modifiedDate = category.ModifiedDate.ToString();
                var modifiedby = category.ModifiedBy;
                
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>  
                                  </tr>", name, image, 
                                  createdDate, createdby, modifiedDate, modifiedby);
            }
            sb.Append(@"
                        </table>
                    </thead>
                </html>");

            return Task.FromResult(sb.ToString());
        }
    }
}


