using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Domain.DataAccess;
using LetsDoIt.Moody.Domain.Entities;

namespace LetsDoIt.Moody.Infrastructure.DataAccess.Abstract
{
    public interface IUserDal:IEntityRepository<User>
    {
    }
}
