using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T: BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
    {

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria); // I.e. would be Where x => x.Model == model
        }

        return query; // This is the query which then goes to the DB 
    }
}