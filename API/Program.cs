using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. // These are things we INJECT into OTHER classes 
builder.Services.AddDbContext<StoreContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
});

builder.Services.AddControllers();

// AddScoped - Service will last for as long as HTTP request is open
builder.Services.AddScoped<IProductRepository, ProductsRepository>();

var app = builder.Build(); // Everything BEFORE this line is a SERVICE. AFTER this line is MIDDLEWARE

// Configure the HTTP request pipeline. // Pipeline a request GOES through before reaching our controller end point (and when going out from endpoint)

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
