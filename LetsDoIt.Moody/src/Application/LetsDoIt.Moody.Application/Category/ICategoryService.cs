using System.Collections.Generic;
using System.Threading.Tasks;


namespace LetsDoIt.Moody.Application.Category
{
    using Domain;

    public interface ICategoryService
    {
        Task<CategoryGetResult> GetCategoriesWithDetails(string versionNumber);

        Task<CategoryGetResult> GetCategories();

        Task<IEnumerable<CategoryDetails>> GetCategoryDetails(int categoryId);

        Task InsertAsync(string name, int order, byte[] image);

        Task InsertCategoryDetailsAsync(int categoryId, int order, string image);

        Task UpdateAsync(int id, string name, int order, byte[] image);

        Task UpdateCategoryDetailsAsync(int id, int order, byte[] image);

        Task DeleteAsync(int id);

        Task DeleteCategoryDetailsAsync(int id);

    }
}