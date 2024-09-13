using AMSaiian.Shared.Web.Middlewares;
using Auth;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.Seeding.Initializers;
using Serilog;
using Taskio;
using Taskio.Application;
using Taskio.Infrastructure;
using Taskio.Infrastructure.Persistence.Seeding.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Host.UseLogging(builder.Services, builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddApplicationInfrastructure(builder.Configuration, "Application");
builder.Services.AddAuthProvider(builder.Configuration, "Auth");
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Initialise and seed database
using (var scope = app.Services.CreateScope())
{
    var appDbInitializer = scope.ServiceProvider
        .GetRequiredService<IAppDbContextInitializer>();

    var authDbInitializer = scope.ServiceProvider
        .GetRequiredService<IAppIdentityDbContextInitializer>();

    await appDbInitializer.ApplyDatabaseStructure();
    await authDbInitializer.ApplyDatabaseStructure();

    if (bool.TryParse(builder.Configuration.GetSection("Seeding").Value, out bool value) && value)
    {
        await appDbInitializer.SeedAsync();
        await authDbInitializer.SeedAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();

namespace Taskio
{
    public partial class Program;
}
