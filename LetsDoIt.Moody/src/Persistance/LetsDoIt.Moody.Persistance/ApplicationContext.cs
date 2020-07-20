using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Persistance
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
                  : base(options)
        {
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
