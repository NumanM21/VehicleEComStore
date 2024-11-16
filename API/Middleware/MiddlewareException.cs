using System.Net;
using System.Text.Json;
using API.Errors;
using Microsoft.AspNetCore.Components.Routing;

namespace API.Middleware;

// IHostEnvironment -> Can access to see if in prod or development

public class MiddlewareException(IHostEnvironment environment, RequestDelegate next )
{
    public async Task InvokeAsync(HttpContext context) // HAS to be called InvokeAsync() in middleware (middleware looks for this method name)
    {
        try
        {
            await next(context); // Look documentation for middleware diagram (next moves onto next piece of logic in our middleware)
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e, environment);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment hostEnvironment)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // will set this to 500

        var response = environment.IsDevelopment()
            ? new ApiErrorResponse(context.Response.StatusCode, exception.Message, exception.StackTrace ?? string.Empty)
            : new ApiErrorResponse(context.Response.StatusCode, exception.Message, "Internal Server Error"); //  since we are in prod, want to keep output minimal

        var jsonOptions = new JsonSerializerOptions(); 
        jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Default case is CamelCase for API, but not OUTSIDE of an API

        var json = JsonSerializer.Serialize(response, jsonOptions);

        return context.Response.WriteAsync(json);
    }
}