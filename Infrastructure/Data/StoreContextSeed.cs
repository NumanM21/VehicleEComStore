using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    //  Static -> Can use method without having to create a new instance of the StoreContextSeed class
    public static async Task SeedAsync(StoreContext context)
    {
        // Seed if we have no products in our DB
        if (!context.Products.Any())
        {
            // Path good for when we publish application
            var prodData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/Products.json");
            
            // Deserialize from Json into Product Classes we use in C# (so we can use them)
            var products = JsonSerializer.Deserialize<List<Product>>(prodData);

            if (products == null) return;
            
            context.Products.AddRange(products);

            await context.SaveChangesAsync();

        }
        
    }
}