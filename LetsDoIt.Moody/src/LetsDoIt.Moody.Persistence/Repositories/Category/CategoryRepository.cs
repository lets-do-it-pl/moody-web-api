using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistence.Repositories.Category
{
    using Entities;
    using Base;

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly IRepository<CategoryDetail> _categoryDetailRepository;

        public CategoryRepository(
            ApplicationContext context,
            IRepository<CategoryDetail> categoryDetailRepository)
            : base(context)
        {
            _categoryDetailRepository = categoryDetailRepository;
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

        public async Task<List<Category>> GetListWithDetailsAsync()
        {
            var categories = await Context.Categories.Where(c => !c.IsDeleted).ToListAsync();

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

        public override async Task DeleteAsync(Category entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsDeleted = true;

            await base.DeleteAsync(entity);

            var categoryDetails = await _categoryDetailRepository.GetListAsync(c => c.CategoryId == entity.Id && !c.IsDeleted);

            if (categoryDetails.Any())
            {
                categoryDetails.ForEach(cd => cd.ModifiedBy = entity.ModifiedBy);
                categoryDetails.ForEach(cd => cd.Order -= 1000);
                await _categoryDetailRepository.BulkDeleteAsync(categoryDetails);
            }
        }
    }
}