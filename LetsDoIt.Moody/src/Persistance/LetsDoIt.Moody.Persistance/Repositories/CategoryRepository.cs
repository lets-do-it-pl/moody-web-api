using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain;
    using Base;  

    public class CategoryRepository : EntityRepositoryBase<Category>
    {
        public CategoryRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public override async Task<List<Category>> GetListAsync(Expression<Func<Category, bool>> filter = null)
        {
            var categories = await base.GetListAsync(filter);

            if(categories == null || categories.Count == 0)
            {
                return categories;
            }

            foreach (var category in categories)
            {
                category.CategoryDetails = category.CategoryDetails.Where(cd => !cd.IsDeleted).ToList();
            }

            return categories;
        }
    }
}