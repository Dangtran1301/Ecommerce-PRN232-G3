using Microsoft.EntityFrameworkCore;
using UserService.API;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
// 🟢 Presentation (API)
services.AddApiPresentation();

// 🟢 Application Layer
services.AddApplicationServices();

// 🟢 Infrastructure Layer
services.AddInfrastructureServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
    app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
    {
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
    }
    else
    {
        await db.Database.MigrateAsync();
    }
}
app.UseAuthorization();

app.MapControllers();

app.Run();