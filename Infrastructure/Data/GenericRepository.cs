﻿using Core.Entities;
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

    public async Task<T?> GetEntityWithSpecification(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(); // Return first item or default (which would be null -> Check for this in our controller)
    }

    public async Task<IReadOnlyList<T>> GetEntitiesWithSpecification(ISpecification<T> specification)
    {// This is a Queryable at this point, so we use ToListAsync();
        return await ApplySpecification(specification).ToListAsync(); // Spec would filter our list, and return the list we are looking for
    }

    public async Task<TResult?> GetEntityWithSpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetEntitiesWithSpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).ToListAsync(); // Specification will have T and TResult, so will automatically choose the overloaded method we created
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

    public async Task<int> TotalCountAsync(ISpecification<T> specification)
    {
        var query = context.Set<T>().AsQueryable();

        query = specification.ApplyCriteria(query);

        return await query.CountAsync(); // Now for PAGINATION, we make 2 REQUESTS to DB (1 for LIST of products, 2 for COUNT of products)
    }

    // Helper method for specification -> We use our EVALUATOR here to break down the expression into an IQueryable to then pass into our DB to retrieve what we want
    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        // Can call .GetQuery since we made the method static
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
    }
    
    // This version if for projection (where we want to pull distinct models and brands of cars)
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        // Can call .GetQuery since we made the method static
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), specification);
    }
}