using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Feature Management

// Via appsettings.json
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureManagement")).AddFeatureFilter<PercentageFilter>();

// Via AzureAppConfiguration
//builder.Configuration.AddAzureAppConfiguration(options =>
//    options.Connect(builder.Configuration["ConnectionStrings:AppConfig"])
//        .UseFeatureFlags(featureFlagOptions => {
//            featureFlagOptions.CacheExpirationInterval = TimeSpan.FromMinutes(1);
//        })
//    );

//builder.Services.AddAzureAppConfiguration();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Feature Management

// Via AzureAppConfiguration
//app.UseAzureAppConfiguration();

#endregion

app.Run();
