using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // We don't have to use attribute in out 'create' such as [FromBody] Product prod -> This attribute does automatic model binding for us
    [Route("api/[controller]")] // routing via api/products ([] placeholder for class name, excluding the controller) 
    public class ProductsController(IProductRepository productRepository) : ControllerBase // Have to use interface since we specified this FIRST in our service
    {
        // ActionResult -> Allows us to return HTTP type of responses
        // Task -> Used with async to delegate work until we reach await
        // IEnumerable -> List, with defined type (product) which we can return through the action result
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            // Need to get list of products from DB -> So we use Dependency Injection in Constructor of CLASS to access DB through StoreContext!

            return Ok(await productRepository.GetProductsAsync()); // Ok to remove type error from GetProductAsync
        }

        [HttpGet("{id:int}")] // Specify id in root which has to be type int --> api/products/id  ==> This id from Http root will be passed as a parameter
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost] // Create Product in our DB
        public async Task<ActionResult<Product>> CreateProduct(Product prod)
        {   
            productRepository.AddProduct(prod);

            if (await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new {id = prod.Id}, prod); // new.. has to match the httpget of the GetProduct method above (Gives us a header to location of this product)
            }

            return  BadRequest("Problem in creating a product");
        }

        [HttpPut("{id:int}")] // Update product
        public async Task<ActionResult> UpdateProduct(int id, Product prod) // Not returning anything from ActionResult 
        {
            if (prod.Id != id || !ProductExists(id)) return BadRequest("Product cannot be updated.");

           productRepository.UpdateProduct(prod);

            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Product Update did NOT happen");
        } 

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var prod = await productRepository.GetProductByIdAsync(id);

            if (prod == null) return NotFound("Product not found in Db");

            productRepository.DeleteProduct(prod);

            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Product was not deleted!");

        }

        private bool ProductExists(int id) // Use this in our Update method to check if the id we passing into root MATCHES the product id we want to update
        {
           return productRepository.ProductExists(id); // Similar to before, but doing the check in our repo method rather than here
        }

        
    }
}