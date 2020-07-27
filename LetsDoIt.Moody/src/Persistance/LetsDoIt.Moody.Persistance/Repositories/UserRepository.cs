using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
