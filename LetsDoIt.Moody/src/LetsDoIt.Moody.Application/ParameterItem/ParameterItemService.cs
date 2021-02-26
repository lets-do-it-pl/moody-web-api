using System;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.VersionHistory;
using LetsDoIt.Moody.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Application.ParameterItem
{
    public class ParameterItemService : IParameterItemService
    {
        private readonly IRepository<Persistence.Entities.ParameterItem> _parameterItemRepository;

        public ParameterItemService(IRepository<Persistence.Entities.ParameterItem> parameterItemRepository)
        {
            _parameterItemRepository = parameterItemRepository;
        }

        public async Task<Persistence.Entities.ParameterItem> GetLatestVersionNumberAsync()
        {
            var result = await _parameterItemRepository
                .Get()
                .FirstOrDefaultAsync(p => p.ParameterKey == "CategoryVersionNumber");

            if (result == null)
            {
                throw new Exception("Couldn't get latest version history!");
            }

            return result;
        }

        public async Task UpdateVersionNumberAsync(int userId)
        {
            var entity = await _parameterItemRepository
                .Get()
                .FirstOrDefaultAsync(p => p.ParameterKey == "CategoryVersionNumber");

            entity.ModifiedBy = userId;
            entity.ParameterValue = Guid.NewGuid().ToString();

            await _parameterItemRepository.UpdateAsync(entity);

        }
    }
}
