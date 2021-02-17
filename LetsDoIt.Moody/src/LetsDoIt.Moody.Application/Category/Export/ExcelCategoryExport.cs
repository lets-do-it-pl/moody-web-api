using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

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

        public async Task ExportAsync()
        {
            var categories = await _categoryRepository.GetListWithDetailsAsync();

            var excel = new Excel.Application();
            excel.Visible = true;
            excel.Workbooks.Add();
            Excel.Worksheet workSheet = (Excel.Worksheet)excel.ActiveSheet;

            workSheet.Cells[1, "A"] = "Category No";
            workSheet.Cells[1, "B"] = "Category Name";
            workSheet.Cells[1, "C"] = "Image Count";
            workSheet.Cells[1, "D"] = "Create Date";
            workSheet.Cells[1, "E"] = "Created by";
            workSheet.Cells[1, "F"] = "Modified Date";
            workSheet.Cells[1, "F"] = "Modified by";

            var rows = 1;
            var index = 1;
            foreach (var category in categories)
            {
                rows++;
                workSheet.Cells[rows, "A"] = index++;
                workSheet.Cells[rows, "B"] = category.Name;
                workSheet.Cells[rows, "C"] = categories.Select(c => c.CategoryDetails.Count());
                workSheet.Cells[rows, "D"] = category.CreatedDate;
                workSheet.Cells[rows, "E"] = category.CreatedBy;
                workSheet.Cells[rows, "F"] = category.ModifiedDate;
                workSheet.Cells[rows, "G"] = category.ModifiedBy;
            }

            var numberOfColumns = workSheet.UsedRange.Columns.Count;
            for (int i = 0; i < numberOfColumns; i++)
                ((Excel.Range)workSheet.Columns[i]).AutoFit();
        }
    }
}