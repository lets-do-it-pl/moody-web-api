using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    public interface ICategoryService
    {
        Task<CategoryGetResult> GetCategoriesWithDetails(string versionNumber);

        Task InsertAsync(string name, int order, byte[] image, int userId);

        Task InsertCategoryDetailsAsync(int categoryId, int order, string image, int userId);

        Task UpdateAsync(int id, string name, int order, byte[] image, int userId);

        Task UpdateCategoryDetailsAsync(int id, int order, byte[] image, int userId);

        Task DeleteAsync(int categoryId, int userId);

        Task DeleteCategoryDetailsAsync(int id, int userId);

    }
}