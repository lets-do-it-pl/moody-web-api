using System.Linq;
using System.Net.Http;
using LetsDoIt.Moody.Persistance;
using LetsDoIt.Moody.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Application.IntegrationTests
{
    public class IntegrationTestBase
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {   
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                 typeof(DbContextOptions<ApplicationContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<ApplicationContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            TestClient = appFactory.CreateClient();
        }


        //When Authorization attribute added will be needed

        //protected async Task AuthorizeAsync()
        //{
        //}
    }
}
