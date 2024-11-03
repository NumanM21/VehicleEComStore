using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T> (StoreContext context) : IGenericRepository<T> where T: BaseEntity
{

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetALlAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public void Add(T entity)
    {
        context.Set<T>() // This is the point we set the type
            .Add(entity);
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity); // Attach entity so EF can now track our entity we are passing in
        context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool EntityExists(int id)
    {
       return context.Set<T>().Any(x => x.Id == id); 
    }
}