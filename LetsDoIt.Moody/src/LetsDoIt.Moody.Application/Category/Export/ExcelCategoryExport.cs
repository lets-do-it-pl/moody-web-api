using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using Persistence.Repositories.Category;
    using Data;

    public class ExcelCategoryExport : ICategoryExport
    {
        private const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDataService _dataService;

        public ExcelCategoryExport(ICategoryRepository categoryRepository, IDataService dataService)
        {
            _categoryRepository = categoryRepository;
            _dataService = dataService;
        }

        public async Task<ExportReturnResult> ExportAsync()
        {
            var categories = await _categoryRepository.GetListWithDetailsAsync();
            var users = _dataService.GetUsers();

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

                for (var index = 2; index < categories.Count() + 2; index++)
                {
                    var category = categories[index - 2];

                    workSheet.Cell(index, 1).Value = index + 1;
                    workSheet.Cell(index, 2).Value = category.Name;
                    workSheet.Cell(index, 3).Value = category.CategoryDetails.Count();
                    workSheet.Cell(index, 4).Value = category.CreatedDate.ToShortDateString();
                    workSheet.Cell(index, 5).Value = users.FirstOrDefault(u => u.Id == category.Id).CreatedBy;
                    workSheet.Cell(index, 6).Value = category.ModifiedDate.ToString();
                    workSheet.Cell(index, 7).Value = users.FirstOrDefault(u => u.Id == category.Id).ModifiedBy;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return new ExportReturnResult
                    {
                        Content = content,
                        FileName = fileName,
                        ContentType = ContentType
                    };
                }
            }
        }
    }
}