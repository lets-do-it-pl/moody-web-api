using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistence.Repositories.Base
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        protected readonly ApplicationContext Context;
        private readonly DeleteType _deleteType;

        protected RepositoryBase(
            ApplicationContext context,
            DeleteType deleteType = DeleteType.Soft)
        {
            Context = context;
            _deleteType = deleteType;
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null,
                                                              Expression<Func<TEntity, decimal>> order = null)
        {
            try
            {
                var result = Context.Set<TEntity>();

                if (filter != null)
                {
                    return await result.Where(filter).ToListAsync();
                }

                if (order != null)
                {
                    return await result.OrderBy(order).ToListAsync();
                }

                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}", ex.InnerException);
            }
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return await Context.Set<TEntity>().FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entity: {ex.Message}", ex.InnerException);
            }
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return await Context.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entity or find just one row : {ex.Message}", ex.InnerException);
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return await Context.Set<TEntity>().AnyAsync(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't find any of the items : {ex.Message}", ex.InnerException);
            }
        }

        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                var addedEntity = Context.Entry(entity);

                addedEntity.State = EntityState.Added;

                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}", ex.InnerException);
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                var updatedEntity = Context.Entry(entity);

                updatedEntity.State = EntityState.Modified;

                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}", ex.InnerException);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            try
            {
                var deletedEntity = Context.Entry(entity);

                deletedEntity.State = _deleteType == DeleteType.Soft ? EntityState.Modified : EntityState.Deleted;

                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be deleted: {ex.Message}", ex.InnerException);
            }
        }

        public virtual async Task BulkDeleteAsync(IList<TEntity> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    var deletedEntity = Context.Entry(entity);

                    deletedEntity.State = _deleteType == DeleteType.Soft ? EntityState.Modified : EntityState.Deleted;
                }

                await Context.BulkUpdateAsync(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(TEntity)} list could not be bulk deleted: {ex.Message}", ex.InnerException);
            }
        }
    }
}