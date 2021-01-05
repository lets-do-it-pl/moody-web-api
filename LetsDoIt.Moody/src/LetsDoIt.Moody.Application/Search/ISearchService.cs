using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public interface ISearchService
    {
        Task<string>  AutoCompleteSearch(string searchKey);
       
    }
}
