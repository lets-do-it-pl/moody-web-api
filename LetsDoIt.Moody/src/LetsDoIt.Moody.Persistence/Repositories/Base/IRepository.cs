using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories.Base
{

    public interface IRepository<TEntity> where TEntity : class, new()
    {

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null,
                                         Expression<Func<TEntity, decimal>> order = null);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
        
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);

        IQueryable<TEntity> Get();
        
        Task<TEntity> AddAsync(TEntity entity);
        
        Task<TEntity> UpdateAsync(TEntity entity);
        
        Task DeleteAsync(TEntity entity);

        Task BulkDeleteAsync(IList<TEntity> entities);
    }
}