using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category
{
    public interface ICategoryService
    {
        Task DeleteAsync(int id);
    }
}