using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
// When we make an instance of this class, we have to pass an expression (similar to LINQ expressions) 
{
    /*
     --- This is old method of how to use primary constructors and having to create variable to store parameter values, ^ can now have primary ctor in class name
     private readonly Expression<Func<T, bool>> criteria;

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        this.criteria = criteria;
    }
    */
    
    protected BaseSpecification() : this(null) {}  // This is an 'Empty' primary constructor 

    public Expression<Func<T, bool>>? Criteria => criteria; // assign our criteria passed in to our Criteria 
    
    public Expression<Func<T, object>>? OrderBy { get; private set; } // Expression will have a getter and a private setter (can't be accessed from outside)
    
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    
    public bool IsDistinct { get; private set; }

    // protected so only allow BaseSpecification Class and any deriving classes to have access to this
    protected void AddOrderBy(Expression<Func<T, object>>? expressionOrderBy)
    {
        OrderBy = expressionOrderBy;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>>? expressionOrderByDescending)
    {
        OrderByDescending = expressionOrderByDescending;
    }

    protected void ApplyIsDistinct()
    {
        IsDistinct = true;
    }


    // Need to now EVALUATE our expression passed in (HAS to be done at infrastructure level, BEFORE we go to our Core (DB) and bring back relevant info -> Done in SpecificationEvaluator class 
}

    // Base Specification to support our new ISpecification which takes type T and returns and different result (TResult)
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) : BaseSpecification<T> (criteria), ISpecification<T, TResult>  // ? makes expression optional 
{
    protected BaseSpecification() : this(null) {}  // This is an 'Empty' primary constructor // can put ! after null to override the null warning
    
    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
    
}