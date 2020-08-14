﻿using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Persistance
{
    using Domain;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                  : base(options)
        {
        }

        public ApplicationContext()
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<VersionHistory> VersionHistories { get; set; }
    }
}
