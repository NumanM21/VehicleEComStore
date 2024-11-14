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

        if (specification.IsDistinct)
        {
            query = query.Distinct();
        }

        return query; // This is the query which then goes to the DB 
    }

    public static IQueryable<TResult> GetQuery<TSpecification, TResult>(IQueryable<T> query,
        ISpecification<T, TResult> specification)
    {
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }
        
        
        if (specification.OrderBy != null) 
        {
            query = query.OrderBy(specification.OrderBy); 
        }

        if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }
        
        // Projection

        var selectQuery = query as IQueryable<TResult>;

        if (specification.Select != null)
        {
            selectQuery = query.Select(specification.Select); // Select what we are looking for (i.e. Brands or Models)
        }
        
        // Now we specify that we only want the DISTINCT versions of the selected item 
        if (specification.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }
        
        // ?? Null Coalesce Operator -> If selectQuery is NULL, we execute (return in this case) whatever is to the right of '??'
        return selectQuery ?? query.Cast<TResult>();
    }
}