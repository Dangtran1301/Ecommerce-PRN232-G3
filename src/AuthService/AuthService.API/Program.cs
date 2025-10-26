using AuthService.API;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services
    .AddApiVersioningAndSwagger()
    .AddValidationBehavior()
    .AddAuthInfrastructure(config)
    .AddJwtAuthentication(config)
    ;
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();