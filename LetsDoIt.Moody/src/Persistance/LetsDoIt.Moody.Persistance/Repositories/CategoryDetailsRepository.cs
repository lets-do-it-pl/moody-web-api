namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain;
    using Base;

    public class CategoryDetailsRepository : EntityRepositoryBase<CategoryDetails>
    {
        public CategoryDetailsRepository(ApplicationContext context)
            : base(context)
        {

        }        
    }
}
