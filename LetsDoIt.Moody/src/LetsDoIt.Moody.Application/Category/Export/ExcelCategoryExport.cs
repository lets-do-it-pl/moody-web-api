using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Core.ExcelPackage;

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

        public Task<FileStreamResult> ExportAsync()
        {
            return null;
            //var categories = await _categoryRepository.GetListWithDetailsAsync();

            //var stream = new MemoryStream();

            //// If you are a commercial business and have
            //// purchased commercial licenses use the static property
            //// LicenseContext of the ExcelPackage class :
            ////ExcelPackage.LicenseContext = LicenseContext.Commercial;

            //// If you use EPPlus in a noncommercial context
            //// according to the Polyform Noncommercial license:
            ////ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //using (ExcelPackage package = new ExcelPackage(stream))
            //{
            //    var workSheet = package.Workbook.Worksheets.Add("Sheet1");

            //    workSheet.Cell(1, 1).Value = "Category No";
            //    workSheet.Cell(1, 2).Value = "Category Name";
            //    workSheet.Cell(1, 3).Value = "Image Count";
            //    workSheet.Cell(1, 4).Value = "Create Date";
            //    workSheet.Cell(1, 5).Value = "Created by";
            //    workSheet.Cell(1, 6).Value = "Modified Date";
            //    workSheet.Cell(1, 7).Value = "Modified by";

            //    var rows = 1;
            //    var index = 1;
            //    foreach (var category in categories)
            //    {
            //        rows++;
            //        workSheet.Cell(rows, 1).Value = index++.ToString();
            //        workSheet.Cell(rows, 2).Value = category.Name;
            //        workSheet.Cell(rows, 3).Value = category.CategoryDetails.Select(c => !c.IsDeleted).Count().ToString();
            //        workSheet.Cell(rows, 4).Value = category.CreatedDate.ToString();
            //        workSheet.Cell(rows, 5).Value = category.CreatedBy.ToString();
            //        workSheet.Cell(rows, 6).Value = category.ModifiedDate.ToString();
            //        workSheet.Cell(rows, 7).Value = category.ModifiedBy.ToString();
            //    }

            //    //var numberOfColumns = workSheet.Dimension.Columns;
            //    //for (int i = 0; i < numberOfColumns; i++)
            //    //    workSheet.Column(i).AutoFit();

            //    package.Save();
            //}
            //stream.Position = 0;

            //return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}