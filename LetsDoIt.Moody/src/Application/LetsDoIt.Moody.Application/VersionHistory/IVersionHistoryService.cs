using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    public interface IVersionHistoryService
    {
        Task CreateNewVersionAsync();
    }
}
