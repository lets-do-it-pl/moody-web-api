using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LetsDoIt.Moody.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistance.DataAccess.EfTechnology
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {

        private readonly ApplicationContext _context;

        public EfEntityRepositoryBase(ApplicationContext context)
        {
            _context = context;
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
                return filter == null
                    ? _context.Set<TEntity>().ToList()
                    : _context.Set<TEntity>().Where(filter).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
                return _context.Set<TEntity>().SingleOrDefault(filter);
        }

        public TEntity Add(TEntity entity)
        {
                var addedEntity = _context.Entry(entity);
                addedEntity.State = EntityState.Added;
                _context.SaveChanges();
                return entity;
        }

        public TEntity Update(TEntity entity)
        {
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                _context.SaveChanges();
                return entity;
        }

        public void Delete(TEntity entity)
        {
            entity.isDeleted = true;
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Modified;
                _context.SaveChanges();
        }
    }
}
