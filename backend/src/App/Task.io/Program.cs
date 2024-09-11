using Auth;
using Task.io;
using Task.io.Application;
using Task.io.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseLogging(builder.Services, builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure("");
builder.Services.AddAuthProvider(builder.Configuration, "Auth");
builder.Services.AddApiServices(builder.Configuration);

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

app.MapControllers();

app.Run();
