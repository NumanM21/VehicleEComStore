using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // New .Net 8 way creating a contructor (since we can have multiple, the 'primary' one can be merged with the class name)
    public class StoreContext(DbContextOptions options) : DbContext(options) // options being passed to the BASE DbContext
    {
        // Options in DbContextOptions would be our SQL server string (to connect to)

        // Also need to DEFINE entites we created -> 'Products' will be used as NAME of DB in our SQLServer 
        public DbSet<Product> Products { get; set; }
    }
}