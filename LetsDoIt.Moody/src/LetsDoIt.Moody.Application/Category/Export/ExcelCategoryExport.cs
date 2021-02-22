using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using Persistence.Repositories.Category;

    public class ExcelCategoryExport : ICategoryExport
    {
        private readonly ICategoryRepository _categoryRepository;

        public ExcelCategoryExport(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ExportReturnResult> ExportAsync()
        {
            var categories = await _categoryRepository.GetListWithDetailsAsync();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "categories.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet workSheet = workbook.Worksheets.Add("Categories");

                workSheet.Cell(1, 1).Value = "Category No";
                workSheet.Cell(1, 2).Value = "Category Name";
                workSheet.Cell(1, 3).Value = "Image Count";
                workSheet.Cell(1, 4).Value = "Create Date";
                workSheet.Cell(1, 5).Value = "Created by";
                workSheet.Cell(1, 6).Value = "Modified Date";
                workSheet.Cell(1, 7).Value = "Modified by";

                var rows = 1;
                var index = 1;
                foreach (var category in categories)
                {
                    rows++;
                    workSheet.Cell(rows, 1).Value = index++;
                    workSheet.Cell(rows, 2).Value = category.Name;
                    workSheet.Cell(rows, 3).Value = category.CategoryDetails.Select(c => !c.IsDeleted).Count();
                    workSheet.Cell(rows, 4).Value = category.CreatedDate;
                    workSheet.Cell(rows, 5).Value = category.CreatedBy;
                    workSheet.Cell(rows, 6).Value = category.ModifiedDate;
                    workSheet.Cell(rows, 7).Value = category.ModifiedBy;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    var result = new ExportReturnResult
                    {
                        Content = content,
                        FileName = fileName,
                        ContentType = contentType
                    };
                    return result;
                }
            }
        }
    }
}