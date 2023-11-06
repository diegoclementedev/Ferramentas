using Microsoft.EntityFrameworkCore;

namespace IntegrationsTestsInsideContainerApi;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<EfCoreDbContext>();

        dbContext.Database.Migrate();
    }
}