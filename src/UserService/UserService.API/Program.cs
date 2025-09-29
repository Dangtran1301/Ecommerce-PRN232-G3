using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;
using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.UnitOfWorks.Interfaces;
using UserService.Infrastructure.UnitOfWorks.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContextService(configuration);

services.AddAutoMapper(typeof(UserProfile).Assembly);
services.AddScoped<IUserService, UserService.Application.Services.UserService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
services.AddScoped<IDbContext, UserDbContext>();
services.AddScoped<UserDbContext>();

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

app.Run();