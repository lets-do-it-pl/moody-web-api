namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Domain;
    using Base;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Linq.Expressions;
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class UserTokenRepository : IEntityRepository<UserToken>
    {
        private readonly ApplicationContext _context;

        public UserTokenRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Task<UserToken> AddAsync(UserToken entity)
        {
            throw new NotImplementedException();
        }

        public Task BulkDeleteAsync(IList<UserToken> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(UserToken entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserToken> Get()
        {
            throw new NotImplementedException();
        }

        public Task<UserToken> GetAsync(Expression<Func<UserToken, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserToken>> GetListAsync(Expression<Func<UserToken, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public async Task<UserToken> UpdateAsync(UserToken entity)
        {
            var userToken = await _context.UserTokens.SingleOrDefaultAsync( ut => ut.UserId == entity.UserId);

            userToken.Token = entity.Token;
            userToken.ExpirationDate = entity.ExpirationDate;
            userToken.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return userToken;
        }
    }
}
