using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Domain;

    public interface IVersionHistoryService
    {
        Task CreateNewVersionAsync();

        Task<VersionHistory> GetLatestVersionNumberAsync();
    }
}
