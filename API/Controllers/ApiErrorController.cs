using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ApiErrorController : BaseApiController  // Class to help us with API Error handling -> So we have more information on HTTP responses
{
    [HttpGet("unauthorised")]  // url/api/apierror/unauthorised   // 401
    public IActionResult GetUnauthorised()
    {
        return Unauthorized();

    }
    
    [HttpGet("bad-request")]  // '-' is IMPORTANT when testing and using endpoint!  // 400
    public IActionResult GetBadRequest()
    {
        return BadRequest("This is a bad request");

    }
    
    [HttpGet("not-found")] // 404
    public IActionResult GetNotFound()
    {
        return NotFound();

    }
    
    [HttpGet("internal-server-error")] // 500
    public IActionResult GetInternalServerError()
    {
        throw new Exception("Exception for internal server error");

    }

    [HttpPost("validation-error")]  // 400
    public IActionResult GetValidationError(ProductDto productDto)
    {
        return Ok();

    }

}