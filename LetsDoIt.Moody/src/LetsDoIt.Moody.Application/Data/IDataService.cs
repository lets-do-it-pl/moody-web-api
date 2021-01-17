using System.Threading.Tasks;
using LetsDoIt.Moody.Persistence.StoredProcedures;

namespace LetsDoIt.Moody.Application.Data
{
    public interface IDataService
    {

        Task<AutoCompleteSearch> GetAutoCompleteSearchAsync(string search);
    }
}
