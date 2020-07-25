using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain; 

    public class VersionHistoryRepository : IEntityRepository<VersionHistory>
    {
        public async Task<List<VersionHistory>> GetListAsync(Expression<Func<VersionHistory, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public async Task<VersionHistory> GetAsync(Expression<Func<VersionHistory, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQuerable<VersionHistory> GetAsync(){
            return _context.Set<VersionHistory>();
        }
        
        public async Task<VersionHistory> AddAsync(VersionHistory entity)
        {
            entity.CreateDate = DateTime.Now;

            var addedEntity = _context.Entry(entity);

            addedEntity.State = EntityState.Added;
            
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public async Task<VersionHistory> UpdateAsync(VersionHistory entity)
        {
            throw new NotImplementedException();
        }

    }
}