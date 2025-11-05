using AuthService.API;
using AuthService.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
config
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services
    .AddApiVersioningAndSwagger()
    .AddValidationBehavior()
    .AddAuthInfrastructure(config)
    .AddJwtAuthentication(config);
var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
    app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();