using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories
{
    using Entities;
    using Base;

    public class VersionHistoryRepository : RepositoryBase<VersionHistory>
    {
        public VersionHistoryRepository(ApplicationContext context)
        : base(context, DeleteType.Hard)
        {
        }

        public override Task<VersionHistory> AddAsync(VersionHistory entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            return base.AddAsync(entity);
        }
    }
}