using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Domain; 
    using Persistance.Repositories.Base;    

    public class VersionHistoryService : IVersionHistoryService
    {
        private readonly IEntityRepository<VersionHistory> _versionHistoryRepository;

        public VersionHistoryService(IEntityRepository<VersionHistory> versionHistoryRepository)
        {
            _versionHistoryRepository = versionHistoryRepository;
        }

        public async Task CreateNewVersionAsync(){
            var versionHistory = new VersionHistory{
                VersionNumber = Guid.NewGuid().ToString()                
            };

            await _versionHistoryRepository.AddAsync(versionHistory);
        }
    }
}
