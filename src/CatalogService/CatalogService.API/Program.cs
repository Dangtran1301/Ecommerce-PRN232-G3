using CatalogService.API;
using CatalogService.Application;
using CatalogService.Application.DTOs.Brands;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Infrastructure;
using CatalogService.Infrastructure.Data;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

services.AddApiPresentation();
services.AddApplicationServices();
services.AddInfrastructureServices(configuration);

services.AddControllers()
    .AddOData(opt =>
    {
        var modelBuilder = new ODataConventionModelBuilder();
        modelBuilder.EntitySet<CategoryDto>("ODataCategories");
        modelBuilder.EntitySet<BrandDto>("ODataBrands");

        opt.AddRouteComponents("odata", modelBuilder.GetEdmModel())
           .Filter()
           .Select()
           .OrderBy()
           .Count()
           .Expand()
           .SetMaxTop(100)
           .SkipToken();
    });

var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
//    db.Database.Migrate();
//}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
