using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    public interface ICategoryService
    {
        Task<CategoryGetResult> GetCategories(string versionNumber);

        Task InsertAsync(string name, int order, byte[] image);

        Task UpdateAsync(int id, string name, int order, byte[] image);

        Task DeleteAsync(int id);
    }
}