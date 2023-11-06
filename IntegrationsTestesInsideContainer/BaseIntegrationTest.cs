using Microsoft.Extensions.DependencyInjection;
using IntegrationsTestsInsideContainerApi;
using Microsoft.EntityFrameworkCore;

namespace IntegrationsTestesInsideContainer
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly EfCoreDbContext DbContext;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

            DbContext = _scope.ServiceProvider.GetRequiredService<EfCoreDbContext>();
            DbContext.Database.Migrate();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();
        }
    }
}
