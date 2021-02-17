namespace LetsDoIt.Moody.Application.Category.Export
{
    using Resolvers;

    public class CategoryExportFactory
    {
        private CategoryExportServiceResolver _serviceResolver;

        public CategoryExportFactory(CategoryExportServiceResolver serviceResolver)
        {
             _serviceResolver = serviceResolver;
        }

        public ICategoryExport GetInstance(string type) => _serviceResolver(type);
    }
}
