using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity // BaseEntity -> We created this where all entites will have an Id and all our entites we create derive from this, so no issue
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetALlAsync();
    Task<T?> GetEntityWithSpecification(ISpecification<T> specification);
    Task<IReadOnlyList<T>> GetEntitiesWithSpecification(ISpecification<T> specification);
    Task<TResult?> GetEntityWithSpecification<TResult>(ISpecification<T, TResult> specification); // GetEntityWithSpecification<> (the type parameters '<>', allow us to call the diff methods with same name)
    Task<IReadOnlyList<TResult>> GetEntitiesWithSpecification<TResult>(ISpecification<T,TResult> specification);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveAllAsync();
    bool EntityExists(int id);
    Task<int> TotalCountAsync(ISpecification<T> specification);
}