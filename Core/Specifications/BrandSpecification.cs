using Core.Entities;

namespace Core.Specifications;

public class BrandSpecification : BaseSpecification<Product, string> // Prod what this spec is for, string is what we return for our TResult in specification 
{
    public BrandSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyIsDistinct();
    }
}