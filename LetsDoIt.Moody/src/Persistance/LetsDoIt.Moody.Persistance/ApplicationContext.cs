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
                .HasOne<Category>(c => c.Categories)
                .WithMany(c => c.CategoryDetail)
                .HasForeignKey(c => c.CategoryId);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<VersionHistory> VersionHistories { get; set; }

        public DbSet<CategoryDetails> CategoryDetail {get; set;}
    }
}
