using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Persistance.Repositories.Base;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    public class UserRepository:EntityRepositoryBase<User>
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
