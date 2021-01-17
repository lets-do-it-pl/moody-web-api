using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    using Persistence.Entities;

    public interface ICategoryService
    {
        Task<CategoryGetResult> GetCategoriesWithDetails(string versionNumber);

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<IEnumerable<CategoryDetail>> GetCategoryDetailsAsync(int categoryId);

        Task InsertAsync(string name, decimal order, byte[] image//, int userId
            );

        Task InsertCategoryDetailAsync(int categoryId, decimal order, string image//, int userId
            );

        Task UpdateAsync(int id, string name, decimal order, byte[] image//, int userId
            );

        Task UpdateCategoryDetailsAsync(int id, decimal order, byte[] image//, int userId
            );

        Task DeleteAsync(int categoryId//, int userId
            );

        Task DeleteCategoryDetailsAsync(int id//, int userId
            );

    }
}