using CatalogService.API;
using CatalogService.Application;
using CatalogService.Application.DTOs.Brands;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Infrastructure;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApiPresentation();
services.AddApplicationServices();
services.AddInfrastructureServices(configuration);

builder.Services.AddControllers()
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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();