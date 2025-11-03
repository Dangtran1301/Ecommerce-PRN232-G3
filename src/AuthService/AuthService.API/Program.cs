using AuthService.API;
using AuthService.API.Data;

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();