using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistence
{
    using Entities;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public interface IApplicationContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<Category> Categories { get; set; }
        DbSet<CategoryDetail> CategoryDetails { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<VersionHistory> VersionHistories { get; set; }
        DatabaseFacade Database { get; }
    }
}
