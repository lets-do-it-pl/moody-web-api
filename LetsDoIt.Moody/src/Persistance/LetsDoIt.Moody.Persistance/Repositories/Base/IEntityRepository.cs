  
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LetsDoIt.Moody.Persistance.Repositories.Base
{
    using Domain; 

    public interface IEntityRepository<T> where T : class
    {
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null);

        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        IQuerable<T> Get();
        
        Task<T> AddAsync(T entity);
        
        Task<T> UpdateAsync(T entity);
        
        Task DeleteAsync(T entity);
    }
}