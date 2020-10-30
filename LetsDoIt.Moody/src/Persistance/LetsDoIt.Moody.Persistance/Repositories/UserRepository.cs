using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Base;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class UserRepository : EntityRepositoryBase<User>
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public override Task<User> AddAsync(User entity)
        {
            var userToken = new UserToken
            {
                UserId = entity.Id,
                User = entity
            };

            entity.UserToken = userToken;

            _context.UserTokens.Add(userToken);

            return base.AddAsync(entity);
        }

    }
}
