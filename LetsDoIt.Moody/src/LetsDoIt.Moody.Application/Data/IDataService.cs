using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Data
{
    public interface IDataService
    {
        Task<List<string>> SearchFunctionDatabase(string searchKey);

    }
}
