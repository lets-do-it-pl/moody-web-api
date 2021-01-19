namespace LetsDoIt.Moody.Application.Data
{
    public interface IDataService
    {

        Task<ICollection<SbGeneralSearchResult>> GetGeneralSearchResultAsync(string search);
    }
}
