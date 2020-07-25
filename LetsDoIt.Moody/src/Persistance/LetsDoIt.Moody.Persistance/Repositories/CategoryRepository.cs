namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain;

    public class CategoryRepository : EntityRepositoryBase<Category>
    {
        public CategoryRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}