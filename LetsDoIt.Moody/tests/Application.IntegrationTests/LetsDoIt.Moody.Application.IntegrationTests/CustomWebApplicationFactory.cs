using System;
using System.Linq;
using LetsDoIt.Moody.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Application.IntegrationTests
{
    using Persistance;
    using Persistance.Repositories.Base;
    using Web;

    public class CustomWebApplicationFactory<TStartup>:WebApplicationFactory<Startup> where TStartup : class
    {
        private readonly InMemoryDatabaseRoot _databaseRoot = new InMemoryDatabaseRoot();
        public IEntityRepository<Domain.User> UserRepositoryVar;
        private  ServiceProvider _serviceProvider;
        private  ApplicationContext _dbContext;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<ApplicationContext>));

                if (descriptor != null)
                {
                  services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase(new Guid().ToString(),_databaseRoot);
                });

                _serviceProvider = services.BuildServiceProvider();
                var scope = _serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                _dbContext = scopedServices.GetRequiredService<ApplicationContext>();
                UserRepositoryVar = scopedServices.GetRequiredService<IEntityRepository<Domain.User>>();

                _dbContext.Database.EnsureCreated();
            });
        }

        public void ResetDbForTests()
        {
            var users = _dbContext.Users.ToArray();
            var categories = _dbContext.Categories.ToArray();
            var userTokens = _dbContext.UserTokens.ToArray();
            var versionHistories = _dbContext.VersionHistories.ToArray();

            _dbContext.Users.RemoveRange(users);
            _dbContext.Categories.RemoveRange(categories);
            _dbContext.UserTokens.RemoveRange(userTokens);
            _dbContext.VersionHistories.RemoveRange(versionHistories);

            _dbContext.SaveChanges();
        }

        public string GenerateTempSaveUserTokenForTests()
        {
           return  TemporaryToken.GenerateTemporaryToken();
        }
    }
}
