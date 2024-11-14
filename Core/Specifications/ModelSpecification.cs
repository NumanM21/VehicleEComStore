using Core.Entities;

namespace Core.Specifications;

public class ModelSpecification : BaseSpecification<Product, string>
{
    public ModelSpecification()
    {
        AddSelect(x => x.Model);
        ApplyIsDistinct();
    }
    
}