using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T: BaseEntity // Build up our QUERY expression before returning it
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
    {
        // Check for our filter query
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria); // I.e. would be Where x => x.Model == model
        }
        
        // Check for our OrderBy (sorting) query
        if (specification.OrderBy != null) // Check if we have a specification 
        {
            query = query.OrderBy(specification.OrderBy); // We create our query by passing our expression from specification to LINQ OrderBy 
        }

        if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        return query; // This is the query which then goes to the DB 
    }
}