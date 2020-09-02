using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Domain;    
    using Persistance.Repositories.Base;    

    public class VersionHistoryService : IVersionHistoryService
    {
        private readonly IEntityRepository<VersionHistory> _versionHistoryRepository;
        private readonly IEntityRepository<Category> _categoryRepository;

        public VersionHistoryService(
            IEntityRepository<Category> categoryRepository,
            IEntityRepository<VersionHistory> versionHistoryRepository)
        {
            _versionHistoryRepository = versionHistoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task CreateNewVersionAsync(){
            var versionHistory = new VersionHistory
            {
                VersionNumber = Guid.NewGuid().ToString()                
            };

            await _versionHistoryRepository.AddAsync(versionHistory);
        }

        public async Task<VersionHistory> GetLatestVersionNumberAsync()
        {
            return await _versionHistoryRepository
                        .Get()
                        .OrderByDescending(vh => vh.CreateDate)
                        .FirstOrDefaultAsync();
        }
    }
}
