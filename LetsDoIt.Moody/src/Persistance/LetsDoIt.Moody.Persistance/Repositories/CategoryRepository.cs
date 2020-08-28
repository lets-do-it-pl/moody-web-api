namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain;
    using Base;
    using System.Threading.Tasks;
    using System;
    using System.Linq;

    public class CategoryRepository : EntityRepositoryBase<Category>
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context) 
            : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetListAsync()
        {
            var categories = _context.Categories.Where(c => !c.IsDeleted);
            return (Category)categories;

          //  throw new NotImplementedException();
        }

    }
}