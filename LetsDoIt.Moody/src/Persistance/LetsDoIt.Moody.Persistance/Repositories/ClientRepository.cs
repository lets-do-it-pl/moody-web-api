namespace LetsDoIt.Moody.Persistance.Repositories
{
    using Base;
    using Domain;

    public class ClientRepository : EntityRepositoryBase<Client>
    {
        public ClientRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
