using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistance.Repositories.Base
{

    using Domain;

    public abstract class EntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly ApplicationContext _context;

        public EntityRepositoryBase(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public IQueryable<TEntity> Get()
        {
            return _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;

            var addedEntity = _context.Entry(entity);

            addedEntity.State = EntityState.Added;
            
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;

            var updatedEntity = _context.Entry(entity);

            updatedEntity.State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.IsDeleted = true;

            var deletedEntity = _context.Entry(entity);
            
            deletedEntity.State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
        }
    }
}