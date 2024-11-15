using Core.Entities;

namespace Core.Specifications;
    // These specifications will do the CORE business logic -> We just create it here, then use it in the controller class
public class ProductSpecification : BaseSpecification<Product>
{
    // Can pass an expression into our base constructor (which would take our expression and return a bool)
    public ProductSpecification(ProductSpecParameters prodSpecParams) :
        base( // primary ctor (Older/ Normal way of writing it)
            x =>
                (prodSpecParams.Brands.Count == 0 || prodSpecParams.Brands.Contains(x.Brand)) && //Apply filter if prodSpecBrand is NOT empty OR if prodSpecBrand contains a brand we have 
                (prodSpecParams.Models.Count == 0 || prodSpecParams.Models.Contains(x.Model)) && // if .Any() is empty or no match, we include ALL models
                (prodSpecParams.FuelTypes.Count == 0 || prodSpecParams.FuelTypes.Contains(x.Model)) &&
                (prodSpecParams.Gearbox.Count == 0 || prodSpecParams.Gearbox.Contains(x.Model))
            // Have two expressions, since we are FILTERING here for each. 
        )
    {
        ApplyPagination(prodSpecParams.PageSize,prodSpecParams.PageSize * (prodSpecParams.PageIndex - 1) ); // 4 * 1 -1 = 0, so we skip 0 and we take 4 (for page 1). We then increment index by 1
        
        switch (prodSpecParams.Sort)
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