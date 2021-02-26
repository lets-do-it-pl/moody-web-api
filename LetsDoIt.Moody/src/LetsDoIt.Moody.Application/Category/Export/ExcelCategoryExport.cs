using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using Persistence.Repositories.Category;

    public class ExcelCategoryExport : ICategoryExport
    {
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly ICategoryRepository _categoryRepository;

        public ExcelCategoryExport(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ExportReturnResult> ExportAsync()
        {
            var categories = await _categoryRepository.GetListWithDetailsAsync();
            var fileName = $"Categories {DateTime.UtcNow.ToShortDateString()}.xlsx";

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

                for (var index = 0; index < categories.Count(); index++)
                {
                    workSheet.Cell(index + 2, 1).Value = index + 1;
                    workSheet.Cell(index + 2, 2).Value = categories[index].Name;
                    workSheet.Cell(index + 2, 3).Value = categories[index].CategoryDetails.Select(c => !c.IsDeleted).Count();
                    workSheet.Cell(index + 2, 4).Value = categories[index].CreatedDate;
                    workSheet.Cell(index + 2, 5).Value = categories[index].CreatedBy;
                    workSheet.Cell(index + 2, 6).Value = categories[index].ModifiedDate;
                    workSheet.Cell(index + 2, 7).Value = categories[index].ModifiedBy;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return new ExportReturnResult
                    {
                        Content = content,
                        FileName = fileName,
                        ContentType = contentType
                    };
                }
            }
        }
    }
}