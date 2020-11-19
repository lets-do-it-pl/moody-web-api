using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories
{
    using Entities;
    using Base;

    public class CategoryDetailsRepository : RepositoryBase<CategoryDetail>
    {
        public CategoryDetailsRepository(ApplicationContext context)
            : base(context)
        {
        }

        public override Task<CategoryDetail> AddAsync(CategoryDetail entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            return base.AddAsync(entity);
        }

        public override Task<CategoryDetail> UpdateAsync(CategoryDetail entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            return base.UpdateAsync(entity);
        }

        public override Task DeleteAsync(CategoryDetail entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsDeleted = true;

            return base.DeleteAsync(entity);
        }

        public override Task BulkDeleteAsync(IList<CategoryDetail> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModifiedDate = DateTime.UtcNow;
                entity.IsDeleted = true;
            }

            return base.BulkDeleteAsync(entities);
        }
    }
}
