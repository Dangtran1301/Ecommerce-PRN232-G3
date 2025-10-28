using Microsoft.EntityFrameworkCore;
using UserService.API;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.

// 🟢 Presentation (API)
services.AddApiPresentation();

// 🟢 Application Layer
services.AddApplicationServices();

// 🟢 Infrastructure Layer
services.AddInfrastructureServices(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();