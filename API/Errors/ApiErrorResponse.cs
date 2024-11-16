namespace API.Errors;

// Properties we will return once we get an exception -> Put at top of Middleware pipeline to catch any consequent logic errors thrown

public class ApiErrorResponse (int statusCode, string message, string details)
{
    public int StatusCode { get; set; } = statusCode;
    public string? Message { get; set; } = message;
    public string? Details { get; set; } = details;
}