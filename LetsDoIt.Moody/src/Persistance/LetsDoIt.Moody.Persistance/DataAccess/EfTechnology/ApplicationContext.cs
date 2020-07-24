using LetsDoIt.Moody.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistance.DataAccess.EfTechnology
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
