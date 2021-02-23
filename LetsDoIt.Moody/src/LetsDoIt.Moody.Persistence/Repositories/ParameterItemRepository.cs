using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories
{
    using Entities;
    using Base;

    public class ParameterItemRepository : RepositoryBase<ParameterItem>
    {
        public ParameterItemRepository(ApplicationContext context)
        : base(context)
        {
        }

        public override Task<ParameterItem> AddAsync(ParameterItem entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            return base.AddAsync(entity);
        }
    }
}