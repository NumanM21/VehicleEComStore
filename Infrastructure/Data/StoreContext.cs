using System.Runtime.CompilerServices;
using Core.Entities;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // New .Net 8 way creating a contructor (since we can have multiple, the 'primary' one can be merged with the class name)
    public class StoreContext(DbContextOptions options) : DbContext(options) // options being passed to the BASE DbContext
    {
        // Options in DbContextOptions would be our SQL server string (to connect to)

        // Also need to DEFINE entites we created -> 'Products' will be used as NAME of DB in our SQLServer 
        public DbSet<Product> Products { get; set; }

        // Need to make our StoreContext AWARE of our configurations for entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Good idea to include this call to ensure configs in base class are applied

            
            ///.ApplyConfigurationFromAssembly -> Scans project to find any class implementing the IEntityConfiguration<> interface and applies them
            /// typeof will retrieve the assembly which contains the ProductConfiguration class 
            // .Assembly gets reference to the assembly where ProductConfigurationc class is defined -> So on creation, configs are applied
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        }

    }
}