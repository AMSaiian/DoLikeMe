using AMSaiian.Shared.Web.Middlewares;
using Auth;
using Serilog;
using Taskio;
using Taskio.Application;
using Taskio.Infrastructure;

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
