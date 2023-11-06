using IntegrationsTestsInsideContainerApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using Program = IntegrationsTestsInsideContainerApi.Program;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationsTestesInsideContainer
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Strong_password_123!")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<EfCoreDbContext>));

                services.AddDbContext<EfCoreDbContext>(opt => opt.UseSqlServer(_dbContainer.GetConnectionString(), options =>
                {
                    options.CommandTimeout((int)TimeSpan.FromMinutes(60).TotalSeconds);
                }));
            });
        }

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
