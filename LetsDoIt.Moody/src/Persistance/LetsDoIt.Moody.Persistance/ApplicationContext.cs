using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistance
{
    using Domain;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                  : base(options)
        {            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryDetails>()
                .HasOne(c => c.Category)
                .WithMany(c => c.CategoryDetails)
                .HasForeignKey(c => c.CategoryId);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<VersionHistory> VersionHistories { get; set; }

        public DbSet<CategoryDetails> CategoryDetails { get; set; }
    }
}
