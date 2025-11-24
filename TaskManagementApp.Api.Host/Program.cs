using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagementApp.Infrastructure.Data;
using TaskManagementApp.Services;
using TaskManagementApp.Api;
using System.Text.Json.Serialization; // added

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services with enum string serialization
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure infrastructure & application services via extension
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplicationServices(configuration);

var app = builder.Build();

// Ensure DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}

// Decide whether to enable Swagger: read config flag if present, otherwise enable only in Development
var swaggerFlag = builder.Configuration.GetValue<bool?>("Swagger:Enabled");
var enableSwagger = swaggerFlag ?? app.Environment.IsDevelopment();
if (enableSwagger)
{
    app.UseSwagger();
    // Serve the Swagger UI at the "/swagger" path so visiting "/swagger/index.html" works
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger"; // serve at /swagger
    });
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
