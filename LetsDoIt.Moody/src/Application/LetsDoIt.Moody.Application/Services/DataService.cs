using System;
using Microsoft.EntityFrameworkCore;

namespace LetsDoIt.Moody.Application.Services
{
    using LetsDoIt.Moody.Domain;

    public class DataService : DbContext, IDataService
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=(localdb)\\MSSQLLocalDB;database=MoodyDB;Trusted_Connection=True");
        }
    }
}
