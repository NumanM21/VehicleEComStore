using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    // Static -> Can use method WITHOUT creating new instance of StoreContextSeed Class
    public static async Task SeedAsync(StoreContext context) // Called in Program class on start up of project
    {
        if (!context.Products.Any())
        {
            // Reading(getting) our JSON seed data. || Path specified better for when in production
            var productsJson = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/Products.json");
            
            // Deserialize from JSON into C# product classes
            var products = JsonSerializer.Deserialize<List<Product>>(productsJson);
            
            // JSONSerializer can be null, so need to check
            if (products == null) return;
            
            context.Products.AddRange(products);

            await context.SaveChangesAsync();

        }
    }
    
}