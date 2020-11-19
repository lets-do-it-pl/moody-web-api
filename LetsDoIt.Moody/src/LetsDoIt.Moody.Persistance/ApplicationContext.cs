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
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasConversion(
                    e => e.ToString(),
                    e => Email.Parse(e));

            modelBuilder.Entity<CategoryDetails>()
                .HasOne(c => c.Category)
                .WithMany(c => c.CategoryDetails)
                .HasForeignKey(c => c.CategoryId);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<VersionHistory> VersionHistories { get; set; }

        public DbSet<CategoryDetails> CategoryDetails { get; set; }

        public DbSet<EmailVerificaitonToken> EmailVerificaitonTokens { get; set; }
    }
}
