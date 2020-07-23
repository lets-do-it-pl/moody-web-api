using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Domain.DataAccess;
using LetsDoIt.Moody.Domain.DataAccess.EntityFramework;
using LetsDoIt.Moody.Domain.Entities;
using LetsDoIt.Moody.Infrastructure.DataAccess.Abstract;

namespace LetsDoIt.Moody.Infrastructure.DataAccess
{
    public class EfUserDal: EfEntityRepositoryBase<User>, IUserDal
    {
        public EfUserDal(ApplicationContext context) : base(context)
        {
        }
    }
}
