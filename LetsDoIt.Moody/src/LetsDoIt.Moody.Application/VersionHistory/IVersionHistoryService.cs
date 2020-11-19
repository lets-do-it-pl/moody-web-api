using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Persistence.Entities;

    public interface IVersionHistoryService
    {
        Task CreateNewVersionAsync();

        Task<VersionHistory> GetLatestVersionNumberAsync();
    }
}