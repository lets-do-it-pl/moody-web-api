using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Data
{
    public interface IDataService
    {
        Task<string> SearchFunction(string searchKey);
        
    }
}
