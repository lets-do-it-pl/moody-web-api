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
    }
}