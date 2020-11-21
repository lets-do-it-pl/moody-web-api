using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Persistence.Entities;
    using Persistence.Repositories.Base;

    public class VersionHistoryService : IVersionHistoryService
    {
        private readonly IRepository<VersionHistory> _versionHistoryRepository;

        public VersionHistoryService(IRepository<VersionHistory> versionHistoryRepository)
        {
            _versionHistoryRepository = versionHistoryRepository;
        }

        public async Task CreateNewVersionAsync()
        {

            var versionHistory = new VersionHistory
            {
                VersionNumber = Guid.NewGuid().ToString()
            };

            await _versionHistoryRepository.AddAsync(versionHistory);
        }

        public async Task<VersionHistory> GetLatestVersionNumberAsync()
        {
            var result = await _versionHistoryRepository
                .Get()
                .OrderByDescending(vh => vh.CreatedDate)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("Couldn't get latest version history!");
            }

            return result;
        }
    }
}
