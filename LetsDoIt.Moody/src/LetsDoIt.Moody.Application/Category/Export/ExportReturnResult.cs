namespace LetsDoIt.Moody.Application.Category.Export
{
    public class ExportReturnResult
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}