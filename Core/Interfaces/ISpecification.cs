using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    // Where query (LINQ) -> We make our own specification expression that does this
    Expression<Func<T, bool>>? Criteria { get; } // Expression that takes a type T (specify on use) and returns a bool (Useful for Filtering (similar to Where())
    
    // Useful expression for sorting (OrderBy and OrderByDescending)
    Expression<Func<T, object>>? OrderBy { get; } // Use object since we can order by number (decimal) or name (string)
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDistinct { get; } // Will be used for the .Distinct() in LINQ where we only pull 1 of EACH model and brand
    
    int Take { get; } // Take for number of entities we take for pagination
    int Skip { get; } // For pagination
    bool IsPaginationEnabled { get; }
    IQueryable<T> ApplyCriteria(IQueryable<T> query); // Used to help us get totalCount in pagination

}

// We need this below for PROJECTION (Take type T and return TRESULT) -> Needed for us to GetAllAsync for DISTINCT MODELS and BRANDS
public interface
    ISpecification<T, TResult> : ISpecification<T> // Derive from interface above so we can still support above methods
{
    Expression<Func<T, TResult>>? Select { get; }
}