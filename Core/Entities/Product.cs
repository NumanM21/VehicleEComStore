
namespace Core.Entities
{
    public class Product : BaseEntity
    {
        // EF needs properties to be public
        public required string Name { get; set; } // 'Required' Operator forces us specify 'required' properties when creating new 'Product' instance (remove compiler err)
        public required string Description  { get; set; }
        public decimal Price { get; set; } 
        public int Year { get; set; }
        public required string FuelType { get; set; }
        public required string Gearbox { get; set; }
        public required int Mileage { get; set; }
        public required string Model { get; set; }    
        public required string Brand { get; set; }   
        public required string PictureUrl { get; set; }  
        public int QuantityInStock { get; set; }
        
    }
}