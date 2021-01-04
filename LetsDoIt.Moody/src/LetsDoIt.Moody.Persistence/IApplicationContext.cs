using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistence
{
    using Entities;

    public interface IApplicationContext
    {
       DbSet<Category> Categories { get; set; }
       DbSet<CategoryDetail> CategoryDetails { get; set; }
       DbSet<Client> Clients { get; set; }
       DbSet<User> Users { get; set; }
       DbSet<VersionHistory> VersionHistories { get; set; }
    }
}
