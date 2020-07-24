using LetsDoIt.Moody.Persistance.DataAccess;

namespace LetsDoIt.Moody.Application.Services.CategoryFolder
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Update(int id, string name, int order, byte[] image)
        {
            throw new System.NotImplementedException();
        }
    }
}
