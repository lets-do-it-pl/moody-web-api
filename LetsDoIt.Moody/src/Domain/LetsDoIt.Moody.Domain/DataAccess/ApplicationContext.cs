using LetsDoIt.Moody.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Domain.DataAccess
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext():base()
        {
        }

      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;database=MoodyDB;Trusted_Connection=True",
                x => x.MigrationsAssembly("LetsDoIt.Moody.Web")));
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
