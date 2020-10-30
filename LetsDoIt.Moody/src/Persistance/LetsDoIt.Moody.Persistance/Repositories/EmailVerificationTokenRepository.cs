using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Persistance.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    public class EmailVerificationTokenRepository :IEntityRepository<EmailVerificaitonToken>
    {
        private readonly ApplicationContext _context;

        public EmailVerificationTokenRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Task<List<EmailVerificaitonToken>> GetListAsync(Expression<Func<EmailVerificaitonToken, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public async Task<EmailVerificaitonToken> GetAsync(Expression<Func<EmailVerificaitonToken, bool>> filter)
        {
            return await _context.EmailVerificaitonTokens.FirstOrDefaultAsync(filter);
        }

        public IQueryable<EmailVerificaitonToken> Get()
        {
            return _context.Set<EmailVerificaitonToken>();
        }

        public async Task<EmailVerificaitonToken> AddAsync(EmailVerificaitonToken entity)
        {
            var addedEntity = _context.Entry(entity);

            addedEntity.State = EntityState.Added;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<EmailVerificaitonToken> UpdateAsync(EmailVerificaitonToken entity)
        {
            var emailVerificationToken = await _context.EmailVerificaitonTokens.SingleOrDefaultAsync(evt => evt.UserId == entity.UserId);

            emailVerificationToken.Token = entity.Token;
            emailVerificationToken.ExpirationDate = entity.ExpirationDate;
            emailVerificationToken.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return emailVerificationToken;
        }

        public async Task DeleteAsync(EmailVerificaitonToken entity)
        {
            var addedEntity = _context.Entry(entity);

            addedEntity.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public Task BulkDeleteAsync(IList<EmailVerificaitonToken> entities)
        {
            throw new NotImplementedException();
        }
    }
}
