using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    using Persistence.Entities;

    public interface ICategoryService
    {
        Task<CategoryGetResult> GetCategoriesWithDetailsAsync(string versionNumber);

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<IEnumerable<CategoryDetail>> GetCategoryDetailsAsync(int categoryId);

        Task InsertAsync(string name, byte[] image, int userId, string description = null);

        Task InsertCategoryDetailAsync(int categoryId, string image, int userId);

        Task UpdateAsync(int id, string name, byte[] image, int userId, string description=null);

        Task UpdateCategoryDetailsAsync(int id, byte[] image, int userId);

        Task UpdateOrderAsync(int id, int userId, int? previousId, int? nextId);

        Task DeleteAsync(int categoryId, int userId);

        Task DeleteCategoryDetailsAsync(int id, int userId);

    }
}