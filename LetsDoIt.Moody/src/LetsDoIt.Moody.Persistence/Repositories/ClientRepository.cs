using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Persistence.Repositories
{
    using Base;
    using Entities;

    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(ApplicationContext context)
            : base(context, DeleteType.Hard)
        {
        }

        public override Task<Client> AddAsync(Client entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            return base.AddAsync(entity);
        }
    }
}
