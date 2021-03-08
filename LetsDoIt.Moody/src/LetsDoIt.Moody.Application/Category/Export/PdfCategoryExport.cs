using LetsDoIt.Moody.Persistence.Repositories.Category;
using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category.Export
{
    using DinkToPdf;
    using DinkToPdf.Contracts;
    using System.IO;
    using Data;
    using System.Collections;

    public class PdfCategoryExport : ICategoryExport
    {
        private const string ContentType = "application/pdf";

        private readonly ICategoryRepository _categoryRepository;
        private readonly IConverter _converter;
        private readonly IPdfTemplateGenerator _pdfTemplateGenerator;
        private readonly IDataService _dataService;

        public PdfCategoryExport(
            ICategoryRepository categoryRepository,
            IConverter converter,
            IPdfTemplateGenerator pdfTemplateGenerator,
            IDataService dataService)
        {
            _categoryRepository = categoryRepository;
            _converter = converter;
            _pdfTemplateGenerator = pdfTemplateGenerator;
            _dataService = dataService;
        }
     
        public async Task<ExportReturnResult> ExportAsync()
        {
            var categories = await _categoryRepository.GetListWithDetailsAsync();
            var users = _dataService.GetUsers();

            var fileName = $"Categories {DateTime.UtcNow.ToShortDateString()}.pdf";

            var htmlContent = await _pdfTemplateGenerator.GetHTMLStringAsync(categories, users);
            
            var globalSettings = GetGlobalSettings();
            var objectSettings = GetObjectSettings(htmlContent);

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var content = _converter.Convert(pdf);

            return new ExportReturnResult
            {
                Content = content,
                FileName = fileName,
                ContentType = ContentType
            };

        }

        private static ObjectSettings GetObjectSettings(string content) => new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = content,
            WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory()) },
            HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
            FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        };

        private static GlobalSettings GetGlobalSettings() => new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10 },
            DocumentTitle = "PDF Report"
        };
    }
}
