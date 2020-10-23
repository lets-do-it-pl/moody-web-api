using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LetsDoIt.Moody.Persistance.Repositories.Base;

namespace LetsDoIt.Moody.Persistance.Repositories
{
    public class EmailVerificationTokenRepository :IEntityRepository<EmailVerificationTokenRepository>
    {
        public Task<List<EmailVerificationTokenRepository>> GetListAsync(Expression<Func<EmailVerificationTokenRepository, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<EmailVerificationTokenRepository> GetAsync(Expression<Func<EmailVerificationTokenRepository, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EmailVerificationTokenRepository> Get()
        {
            throw new NotImplementedException();
        }

        public Task<EmailVerificationTokenRepository> AddAsync(EmailVerificationTokenRepository entity)
        {
            throw new NotImplementedException();
        }

        public Task<EmailVerificationTokenRepository> UpdateAsync(EmailVerificationTokenRepository entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(EmailVerificationTokenRepository entity)
        {
            throw new NotImplementedException();
        }

        public Task BulkDeleteAsync(IList<EmailVerificationTokenRepository> entities)
        {
            throw new NotImplementedException();
        }
    }
}
