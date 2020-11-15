using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain;
    using Base;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class VersionHistoryRepository : IEntityRepository<VersionHistory>
    {
        private readonly ApplicationContext _context;

        public VersionHistoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Task<List<VersionHistory>> GetListAsync(Expression<Func<VersionHistory, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<VersionHistory> GetAsync(Expression<Func<VersionHistory, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<VersionHistory, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<VersionHistory> Get()
        {
            return _context.Set<VersionHistory>();
        }
        
        public async Task<VersionHistory> AddAsync(VersionHistory entity)
        {
            entity.CreateDate = DateTime.UtcNow;

            var addedEntity = _context.Entry(entity);

            addedEntity.State = EntityState.Added;
            
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public Task<VersionHistory> UpdateAsync(VersionHistory entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(VersionHistory entity)
        {
            throw new NotImplementedException();
        }

        public Task BulkDeleteAsync(IList<VersionHistory> entities)
        {
            throw new NotImplementedException();
        }
    }
}