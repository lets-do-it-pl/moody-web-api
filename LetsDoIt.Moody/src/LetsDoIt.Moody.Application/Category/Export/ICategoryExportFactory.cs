namespace LetsDoIt.Moody.Application.Category.Export
{
    public interface ICategoryExportFactory
    {
        public ICategoryExport GetInstance(string type);
    }
}
