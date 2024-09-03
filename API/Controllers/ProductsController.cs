using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // routing via api/products ([] placeholder for class name, excluding the controller) 
    public class ProductsController(StoreContext context) : ControllerBase
    {
        private readonly StoreContext context = context;

        // ActionResult -> Allows us to return HTTP type of responses
        // Task -> Used with async to delegate work until we reach await
        // IEnumerable -> List, with defined type (product) which we can return through the action result
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Need to get list of products from DB -> So we use Dependency Injection in Constructor of CLASS to access DB through StoreContext!

            return await context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")] // Specify id in root which has to be type int --> api/products/id  ==> This id from Http root will be passed as a parameter
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost] // Create Product in our DB
        public async Task<ActionResult<Product>> CreateProduct(Product prod)
        {   
            context.Products.Add(prod);

            await context.SaveChangesAsync();

            return prod;
        }
        
    }
}