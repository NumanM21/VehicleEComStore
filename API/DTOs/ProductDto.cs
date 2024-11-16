using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class ProductDto
{
    [Required] // Can use the required attribute (Helps with validation -> Similar to what we done in product entity class
    public string Name { get; set; } = string.Empty; // Doesn't allow us to create a product with empty string (due to [Required], only have this string,empty to remove null warning)

    [Required] 
    public string Description { get; set; } = string.Empty;

    [Range(1000.00, double.MaxValue, ErrorMessage = "Price must be greater than 1000")] // Use range for decimals! 
    public decimal Price { get; set; }
    
    [Required] 
    public int Year { get; set; }
    
    [Required] 
    public string FuelType { get; set; } = string.Empty;
    
    [Required] 
    public string Gearbox { get; set; } = string.Empty;
    
    [Range(0, 400000, ErrorMessage = "Mileage cannot exceed 400,000. Car is worthless :)")]
    public int Mileage { get; set; }

    [Required] 
    public string Model { get; set; } = string.Empty;
    
    [Required]
    public string Brand { get; set; } = string.Empty;
    
    [Required]
    public string PictureUrl { get; set; } = string.Empty;
    
    [Range(1, 10, ErrorMessage = "Car Quantity must be between 1 and 10, Keep store diverse")]
    public int QuantityInStock { get; set; }
}