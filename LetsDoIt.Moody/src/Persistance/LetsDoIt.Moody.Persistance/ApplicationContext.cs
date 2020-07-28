using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Persistance
{    
    using Domain;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
                  : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }   

        public DbSet<User> Users { get; set; }   

        public DbSet<VersionHistory> VersionHistories { get; set; }        
    }
}
