using Microsoft.EntityFrameworkCore;

namespace IntegrationsTestsInsideContainerApi;

public class ProductService
{
    private readonly EfCoreDbContext _dbContext;

    public ProductService(EfCoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Create(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        _dbContext.Add(product);

        await _dbContext.SaveChangesAsync();

        return product.Id;
    }

    public async Task<ProductResponse> GetById(QueryProductRequest request)
    {
        var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);

        ArgumentNullException.ThrowIfNull(product);

        var productResponse = new ProductResponse()
        {
            Id = product.Id,
            Name = product.Name
        };

        return productResponse;
    }

    public async Task<IEnumerable<ProductResponse>> GetAll()
    {
        var products = await _dbContext.Products.AsNoTracking().ToListAsync();

        ArgumentNullException.ThrowIfNull(products);

        var productsResponse = products.Select(x => new ProductResponse()
        {
            Id = x.Id,
            Name = x.Name
        });

        return productsResponse;
    }
}