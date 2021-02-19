using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using Persistence.Repositories.Category;
    using System.IO;

    public class ExcelCategoryExport : ICategoryExport
    {
        private readonly ICategoryRepository _categoryRepository;

        public ExcelCategoryExport(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<FileStreamResult> ExportAsync()
        {
            var categories = await _categoryRepository.GetListWithDetailsAsync();

            var stream = new MemoryStream();

            // If you are a commercial business and have
            // purchased commercial licenses use the static property
            // LicenseContext of the ExcelPackage class :
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                workSheet.Cells[1, 1].Value = "Category No";
                workSheet.Cells[1, 2].Value = "Category Name";
                workSheet.Cells[1, 3].Value = "Image Count";
                workSheet.Cells[1, 4].Value = "Create Date";
                workSheet.Cells[1, 5].Value = "Created by";
                workSheet.Cells[1, 6].Value = "Modified Date";
                workSheet.Cells[1, 7].Value = "Modified by";

                var rows = 1;
                var index = 1;
                foreach (var category in categories)
                {
                    rows++;
                    workSheet.Cells[1, 1].Value = index++;
                    workSheet.Cells[1, 2].Value = category.Name;
                    workSheet.Cells[1, 3].Value = category.CategoryDetails.Select(c => !c.IsDeleted).Count();
                    workSheet.Cells[1, 4].Value = category.CreatedDate;
                    workSheet.Cells[1, 5].Value = category.CreatedBy;
                    workSheet.Cells[1, 6].Value = category.ModifiedDate;
                    workSheet.Cells[1, 7].Value = category.ModifiedBy;
                }

                var numberOfColumns = workSheet.Dimension.Columns;
                for (int i = 0; i < numberOfColumns; i++)
                    workSheet.Column(i).AutoFit();

                package.Save();
            }
            stream.Position = 0;

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}