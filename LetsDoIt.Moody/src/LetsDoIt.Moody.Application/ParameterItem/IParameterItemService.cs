using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Persistence.Entities;

    public interface IParameterItemService
    {
        Task<ParameterItem> GetLatestVersionNumberAsync();

        Task UpdateVersionNumberAsync(int userId);
    }
}
