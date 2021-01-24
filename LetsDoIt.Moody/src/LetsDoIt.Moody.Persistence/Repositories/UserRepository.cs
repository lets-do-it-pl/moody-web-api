using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories
{
    using Entities;
    using Base;

    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationContext context)
            : base(context)
        {
        }

        public override Task<User> AddAsync(User entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            return base.AddAsync(entity);
        }

        public override Task<User> UpdateAsync(User entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            return base.UpdateAsync(entity);
        }

        public override Task DeleteAsync(User entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsDeleted = true;
            entity.Email = Guid.NewGuid().ToString();

            return base.DeleteAsync(entity);
        }

        public override Task BulkDeleteAsync(IList<User> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModifiedDate = DateTime.UtcNow;
                entity.IsDeleted = true;
                entity.Email = Guid.NewGuid().ToString();
            }

            return base.BulkDeleteAsync(entities);
        }
    }
}
