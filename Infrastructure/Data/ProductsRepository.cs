using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductsRepository(StoreContext context) : IProductRepository
    {
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

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? model, string? sort)
        {
            // Can build our query as depending on arguments passed in through our parameters then return that
            var query = context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(x => x.Brand == brand);

            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(x => x.Model == model);


            // Sorting query
            query = sort switch
            {
                "priceAscending" => query.OrderBy(x => x.Price),
                "priceDescending" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };


            return await query.ToListAsync();
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