using Core.Entities;

namespace Core.Specifications;
    // These specifications will do the CORE business logic -> We just create it here, then use it in the controller class
public class ProductFilterSpecification : BaseSpecification<Product>
{
    // Can pass an expression into our base constructor (which would take our expression and return a bool)
    public ProductFilterSpecification(string? brand, string? model) : base(                 // primary ctor (Older/ Normal way of writing it)
        x => 
            (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) && 
            (string.IsNullOrWhiteSpace(model) || x.Model == model)
            // Have two expressions, since we are FILTERING here for each. 
        )
    { } // Empty code block for the constructor
}