using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController] // We don't have to use attribute in out 'create' such as [FromBody] Product prod -> This attribute does automatic model binding for us
    [Route("api/[controller]")] // routing via api/products ([] placeholder for class name, excluding the controller) 
    public class ProductsController(IGenericRepository<Product>  repository) : ControllerBase // Have to use interface since we specified this FIRST in our service
    // Here is where we can implement our Generic Repo, but now we give it a type (since this is a PRODUCTS controller!)
    {
        // ActionResult -> Allows us to return HTTP type of responses
        // Task -> Used with async to delegate work until we reach await
        // IEnumerable -> List, with defined type (product) which we can return through the action result
        
        [HttpGet]  // We would have to use [FromQuery so API knows to look for query STRINGS, but since we are using api/[Controller], this is done for us
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? model, string? sort)
        {
            // Need to get list of products from DB -> So we use Dependency Injection in Constructor of CLASS to access DB through StoreContext!
            
            // Create our specification (expression to what we want)
            var spec = new ProductFilterSpecification(brand, model);
            
            // Pass our spec to become an expression to retrieve the relevant products from DB
            var prodWhichMeetSpec = await repository.GetEntitiesWithSpecification(spec);
            
            return Ok(prodWhichMeetSpec); // Ok to remove type error
        }

        [HttpGet("{id:int}")] // Specify id in root which has to be type int --> api/products/id  ==> This id from Http root will be passed as a parameter
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost] // Create Product in our DB
        public async Task<ActionResult<Product>> CreateProduct(Product prod)
        {   
            repository.Add(prod);

            if (await repository.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new {id = prod.Id}, prod); // new.. has to match the httpget of the GetProduct method above (Gives us a header to location of this product)
            }

            return  BadRequest("Problem in creating a product");
        }

        [HttpPut("{id:int}")] // Update product
        public async Task<ActionResult> UpdateProduct(int id, Product prod) // Not returning anything from ActionResult 
        {
            if (prod.Id != id || !ProductExists(id)) return BadRequest("Product cannot be updated.");

           repository.Update(prod);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Product Update did NOT happen");
        } 

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var prod = await repository.GetByIdAsync(id);

            if (prod == null) return NotFound("Product not found in Db");

            repository.Delete(prod);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Product was not deleted!");

        }

        private bool ProductExists(int id) // Use this in our Update method to check if the id we passing into root MATCHES the product id we want to update
        {
           return repository.EntityExists(id); // Similar to before, but doing the check in our repo method rather than here
        }
        
        //TODO: Need to implement these with specification pattern -> Can't be done with generics!
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok();
        }
        //TODO: Need to implement these with specification pattern -> Can't be done with generics!
        [HttpGet("models")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetModels()
        {
            return Ok();
        }


    }
}