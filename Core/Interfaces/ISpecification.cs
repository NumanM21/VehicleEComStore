using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    // Where query (LINQ) -> We make our own specification expression that does this
    Expression<Func<T, bool>>? Criteria { get; } // Expression that takes a type T (specify on use) and returns a bool 
    
}