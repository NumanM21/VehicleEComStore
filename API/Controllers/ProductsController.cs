using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // We don't have to use attribute in out 'create' such as [FromBody] Product prod -> This attribute does automatic model binding for us
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

        [HttpPut("{id:int}")] // Update product
        public async Task<ActionResult> UpdateProduct(int id, Product prod) // Not returning anything from ActionResult 
        {
            if (prod.Id != id || !ProductExists(id)) return BadRequest("Product cannot be updated.");

            // EF doesn't know prod we pass in is an ENTITY so we have to tell EF to TRACK the entity we passed in and it's being modified
            context.Entry(prod).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return NoContent();
        } 

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var prod = await context.Products.FindAsync(id);

            if (prod == null) return NotFound("Product not found in Db");

            context.Products.Remove(prod); // EF will now be tracking this removal

            await context.SaveChangesAsync(); // Updates the Db!

            return NoContent();

        }

        private bool ProductExists(int id) // Use this in our Update method to check if the id we passing into root MATCHES the product id we want to update
        {
           return context.Products.Any(x => x.Id == id);
        }

        
    }
}