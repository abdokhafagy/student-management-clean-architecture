using Scalar.AspNetCore;
using StudentManagement.Application.Extensions;
using StudentManagement.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// add scalar
builder.Services.AddOpenApi();

// add connection string .
builder.Services.AddInfrastructure(builder.Configuration);
// add service from Application layer
builder.Services.AddApplication();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// seed data 
//var scope = app.Services.CreateScope();
//var seeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
//await seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
