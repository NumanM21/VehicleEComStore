using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    // Where query (LINQ) -> We make our own specification expression that does this
    Expression<Func<T, bool>>? Criteria { get; } // Expression that takes a type T (specify on use) and returns a bool (Useful for Filtering (similar to Where())
    
    // Useful expression for sorting (OrderBy and OrderByDescending)
    Expression<Func<T, object>>? OrderBy { get; } // Use object since we can order by number (decimal) or name (string)
    Expression<Func<T, object>>? OrderByDescending { get; }
}