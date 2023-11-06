using IntegrationsTestsInsideContainerApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EfCoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapPost("api/products", async (CreateProductRequest request, ProductService productService) =>
{
    var productId = await productService.Create(request);
    return Results.Ok(productId);
});

app.MapGet("api/products", async (ProductService productService) =>
{
    var productResponse = await productService.GetAll();

    return Results.Ok(productResponse);
});

app.MapGet("api/products/{productId}", async (Guid productId, ProductService productService) =>
{
    var productResponse = await productService.GetById(new QueryProductRequest { Id = productId });

    return Results.Ok(productResponse);
});

app.UseHttpsRedirection();

app.Run();

namespace IntegrationsTestsInsideContainerApi
{
    public partial class Program { }
}