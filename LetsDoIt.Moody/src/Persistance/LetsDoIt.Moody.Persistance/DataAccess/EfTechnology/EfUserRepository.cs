using LetsDoIt.Moody.Domain.Entities;

namespace LetsDoIt.Moody.Persistance.DataAccess.EfTechnology
{
    public class EfUserRepository: EfEntityRepositoryBase<User>, IUserRepository
    {
        public EfUserRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
