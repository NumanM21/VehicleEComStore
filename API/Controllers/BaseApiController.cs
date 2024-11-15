using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] // We don't have to use attribute in out 'create' such as [FromBody] Product prod -> This attribute does automatic model binding for us
[Route("api/[controller]")] // routing via api/products ([] placeholder for class name, excluding the controller) 
public class BaseApiController : ControllerBase
{
    // Protected -> Available for this controller and any controllers which derive from it  --> Method also allows other controllers to take advantage of pagination (DRY + SOLID principle)
    protected async Task<ActionResult> CreatedPaginatedResult<T>(IGenericRepository<T> repository,
        ISpecification<T> specification, int pageIndex, int pageSize) where T : BaseEntity // Can add a constraint
    {
        // Pass our spec to become an expression to retrieve the relevant products from DB (this is the Query 1 we make to DB for LIST of products)
        var prodWhichMeetSpec = await repository.GetEntitiesWithSpecification(specification);
            
        // Pass TotalCount query to retrieve COUNT of products being returned after filtering for pagination (this is query 2 we make to DB)
        var totalCount = await repository.TotalCountAsync(specification);

        // Combines pulling the filtered products in a IReadOnlyList and also tracking the pagination so we know which page to be one for the client and how many products we are displaying/ pulling
        var pagination = new Pagination<T>(pageIndex, pageSize, totalCount, prodWhichMeetSpec);

        return Ok(pagination); // Ok to remove type error

    }

}