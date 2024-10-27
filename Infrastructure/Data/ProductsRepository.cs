
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductsRepository(StoreContext context) : IProductRepository
    {
        public async Task<IReadOnlyList<string>> GetModelsAsync()
        {
            return await context.Products.Select(x => x.Model)
                .Distinct()
                .ToListAsync();
        }

        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<Product?> GetProductByIdAsync(int Id)
        {
            return await context.Products.FindAsync(Id);
        }

        public async  Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(x => x.Brand)
                .Distinct() // Creates a DISTINCT list of brands which we select from our products
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
           return await context.Products.ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0; //SaveChangesAsync returns an int (# of changes made)
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }
    }
}