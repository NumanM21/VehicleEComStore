
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(); // Use IReadOnly since we are ONLY retrieving, not modifying our products returned
        Task<Product?> GetProductByIdAsync(int Id); // Use ? since this could return a 'null' if incorrect id supplied
        // Add, Update and Delete are NOT async (since we are not interacting with DB, this only happens when we call .SaveChangesAsync())
        // When we do the above, we are just adding the entity to EF Tracking
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        bool ProductExists(int id);
        Task<bool> SaveChangesAsync(); // Can use this method to check if our other methods ran to completion
        
    }
}