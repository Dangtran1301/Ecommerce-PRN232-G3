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

builder.Configuration
    .AddJsonFile("ocelot.routes.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.swagger.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<NoBufferingHandler>(true);

builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

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

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri($"https://localhost:{config["AUTH_PORT"]}/health"), name: "auth-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["USER_PORT"]}/health"), name: "user-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["PRODUCT_PORT"]}/health"), name: "product-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["CATALOG_PORT"]}/health"), name: "catalog-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["CART_PORT"]}/health"), name: "cart-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["ORDER_PORT"]}/health"), name: "order-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["PAYMENT_PORT"]}/health"), name: "payment-service")
    .AddUrlGroup(new Uri($"https://localhost:{config["STOCK_PORT"]}/health"), name: "stock-service");

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();
app.Run();