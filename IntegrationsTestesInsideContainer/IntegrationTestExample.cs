using IntegrationsTestsInsideContainerApi;

namespace IntegrationsTestesInsideContainer;

public class IntegrationTestExample : BaseIntegrationTest 
{
    public IntegrationTestExample(IntegrationTestWebAppFactory factory) : base(factory)
    {
            
    }

    [Fact]
    public async Task Create_ShouldCreateProduct()
    {
        //Arrange
        var productService = new ProductService(DbContext);
        var productName = "ProductOne";

        //Act
        var productIdInserted = await productService.Create(new CreateProductRequest() { Name = productName });

        //Assert
        var productDatabase = DbContext.Products.FirstOrDefault(p => p.Name == productName && p.Id == productIdInserted);
        Assert.NotNull(productDatabase);
    }
}