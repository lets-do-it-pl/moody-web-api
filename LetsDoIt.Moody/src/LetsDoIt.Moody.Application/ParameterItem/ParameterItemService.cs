using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Application.VersionHistory
{
    using Persistence.Entities;
    using Persistence.Repositories.Base;

    public class ParameterItemService : IParameterItemService
    {
        private readonly IRepository<ParameterItem> _parameterItemRepository;

        public ParameterItemService(IRepository<ParameterItem> parameterItemRepository)
        {
            _parameterItemRepository = parameterItemRepository;
        }

        public async Task<ParameterItem> GetLatestVersionNumberAsync()
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
