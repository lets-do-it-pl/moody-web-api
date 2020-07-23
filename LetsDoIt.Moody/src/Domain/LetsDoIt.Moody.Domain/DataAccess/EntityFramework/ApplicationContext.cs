using LetsDoIt.Moody.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Domain.DataAccess.EntityFramework
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
