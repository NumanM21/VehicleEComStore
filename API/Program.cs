using System.Formats.Asn1;
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

// Build our migration from our Seed Data
try
{
    // Using -> Any code using 'applicationScope', framework will be disposed off along with its dependent services since we are not using Dependency Injection for this service
    using var applicationScope = app.Services.CreateScope();
    
    // Create reference to our service
    var services = applicationScope.ServiceProvider;

    // Service Locator Pattern 
    var context = services.GetRequiredService<StoreContext>();

    // Creates our DB
    await context.Database.MigrateAsync();
    
    // Now we can Seed our data into the DB
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception e)
{
    Console.WriteLine("Exception in our Try in Program.cs for our Migration" + e);
    throw;
}

app.Run();
