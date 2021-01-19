using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LetsDoIt.Moody.Persistence
{
    using Entities;

    public interface IApplicationContext
    {
        DatabaseFacade Database { get; }   
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<Category> Categories { get; set; }
        DbSet<CategoryDetail> CategoryDetails { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<VersionHistory> VersionHistories { get; set; }
    }
}
