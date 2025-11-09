using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.API.BackgroundJobs;
using OrderService.API.Clients;
using OrderService.API.Clients.Interfaces;
using OrderService.API.Clients.Mocks;
using OrderService.API.Data;
using OrderService.API.Repositories.Interfaces;
using OrderService.API.Services.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SharedKernel.Infrastructure.Data.Interfaces.IDbContext, OrderDbContext>();

builder.Services.AddScoped<IOrderRepository, OrderService.API.Repositories.Implementations.OrderRepository>();
builder.Services.AddScoped(typeof(ISpecificationRepository<>), typeof(SpecificationRepository<>));
builder.Services.AddScoped(typeof(IDynamicRepository<>), typeof(DynamicRepository<>));
//builder.Services.AddHttpClient<IProductClient, MockProductClient>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7080/api/v1/catalog/Products/");
//});
builder.Services.AddSingleton<IProductClient, MockProductClient>();

builder.Services.AddScoped<IOrderService, OrderService.API.Services.OrderService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = false;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();