using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;
using Gateway.API;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var environment = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile($"ocelot.{environment}.json", optional: true, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<NoBufferingHandler>(true);

builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
            )
        };
    });

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();
app.Run();