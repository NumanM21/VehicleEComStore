using Core.Entities;

namespace Core.Specifications;
    // These specifications will do the CORE business logic -> We just create it here, then use it in the controller class
public class ProductSpecification : BaseSpecification<Product>
//TODO: Need to add filter for remaining fields (FuelType, Gearbox) (Expend here + ProdController
{
    // Can pass an expression into our base constructor (which would take our expression and return a bool)
    public ProductSpecification(string? brand, string? model, string? sort) :
        base( // primary ctor (Older/ Normal way of writing it)
            x =>
                (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
                (string.IsNullOrWhiteSpace(model) || x.Model == model)
            // Have two expressions, since we are FILTERING here for each. 
        )
    {
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            case "mileageAsc":
                AddOrderBy(x => x.Mileage);
                break;
            case "mileageDesc":
                AddOrderByDescending(x => x.Mileage);
                break;
            case "yearAsc":
                AddOrderBy(x => x.Year);
                break;
            case "yearDesc":
                AddOrderByDescending(x => x.Year);
                break;
            case "quantityAsc":
                AddOrderBy(x => x.QuantityInStock);
                break;
            case "quantityDesc":
                AddOrderByDescending(x => x.QuantityInStock);
                break;
            default: // Order by Alphabet if nothing in sort
                AddOrderBy(x => x.Name);
                break;
        }
    } 
}