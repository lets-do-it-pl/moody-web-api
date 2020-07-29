using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    public interface ICategoryService
    {
        Task DeleteAsync(int id);
        Task InsertAsync(string name, int order, byte[] image);
    }
}