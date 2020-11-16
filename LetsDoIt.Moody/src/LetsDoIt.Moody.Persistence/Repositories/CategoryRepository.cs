using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistence.Repositories
{
    using Entities;
    using Base;

    public class CategoryRepository : RepositoryBase<Category>
    {
        private readonly IRepository<CategoryDetail> _categoryDetailRepository;

        public CategoryRepository(
            ApplicationContext context,
            IRepository<CategoryDetail> categoryDetailRepository)
            : base(context)
        {
            _categoryDetailRepository = categoryDetailRepository;
        }

        public override async Task<List<Category>> GetListAsync(Expression<Func<Category, bool>> filter = null)
        {
            var categories =
                filter == null
                ? await Context.Set<Category>().ToListAsync()
                : await Context.Set<Category>().Where(filter).ToListAsync();

            if (categories == null || categories.Count == 0)
            {
                return categories;
            }

            foreach (var category in categories.Where(category => category.CategoryDetails != null))
            {
                category.CategoryDetails = category.CategoryDetails.Where(cd => !cd.IsDeleted).ToList();
            }

            return categories;
        }

        public override Task<Category> AddAsync(Category entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            return base.AddAsync(entity);
        }

        public override Task<Category> UpdateAsync(Category entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            return base.UpdateAsync(entity);
        }

        public override Task BulkDeleteAsync(IList<Category> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModifiedDate = DateTime.UtcNow;
                entity.IsDeleted = true;
            }

            return base.BulkDeleteAsync(entities);
        }

        public override async Task DeleteAsync(Category entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsDeleted = true;

            if (entity.CategoryDetails != null)
            {
                var categoryDetails = await _categoryDetailRepository.GetListAsync(c => c.CategoryId == entity.Id && !c.IsDeleted);

                await _categoryDetailRepository.BulkDeleteAsync(categoryDetails);
            }

            await base.DeleteAsync(entity);
        }
    }
}