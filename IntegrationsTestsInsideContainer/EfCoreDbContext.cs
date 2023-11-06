using Microsoft.EntityFrameworkCore;

namespace IntegrationsTestsInsideContainerApi;

public class EfCoreDbContext : DbContext
{
    public EfCoreDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Product> Products { get; set; }
}