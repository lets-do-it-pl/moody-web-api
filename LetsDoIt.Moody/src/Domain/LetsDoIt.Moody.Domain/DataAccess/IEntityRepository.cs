using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LetsDoIt.Moody.Domain.Entities.Abstract;

namespace LetsDoIt.Moody.Domain.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity
    {
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);

    }
}
