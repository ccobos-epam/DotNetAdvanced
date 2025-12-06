using DomainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Utilities.Pagination;
using System.Text;

namespace DataAccess;

public interface ICategoryRepository 
{
    Task<CategoryEntity?> GetSingle(Guid id);

    Task<PaginatedResult<CategoryEntity>> List(
        IList<Expression<Func<CategoryEntity, bool>>> filteringConditions,
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> sortingConditions,
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> paginationCondition);


    Task AddSingle(CategoryEntity entity);
    Task UpdateSingle(CategoryEntity entity);
    Task<bool> DeleteSingle(Guid id);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(
        AppDbContext appDbContext
        )
    {
        _appDbContext = appDbContext;
    }

    public async Task AddSingle(CategoryEntity entity)
    {
        _ = _appDbContext.Categories.Add(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteSingle(Guid id)
    {
        var entityToDelete = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
        if (entityToDelete == null)
            return await Task.FromResult(false);        
        _appDbContext.Remove(entityToDelete);
        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<CategoryEntity?> GetSingle(Guid id)
    {
        var entityToReturn = await _appDbContext.Categories.Include(x => x.ParentCategory).FirstOrDefaultAsync(x => x.Id == id);
        return entityToReturn;
    }

    public async Task UpdateSingle(CategoryEntity entity)
    {
        _appDbContext.Categories.Update(entity);
        await _appDbContext.SaveChangesAsync(); 
    }

    public async Task<PaginatedResult<CategoryEntity>> List(
        IList<Expression<Func<CategoryEntity, bool>>> filteringConditions,
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> sortingConditions,
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> paginationCondition)
    {
        var query = _appDbContext.Categories.Include(x => x.ParentCategory).AsQueryable();

        foreach (var item in filteringConditions)
            query = query.Where(item);

        int totalItemsFound = await query.CountAsync();

        query = sortingConditions(query);
        query = paginationCondition(query);

        var data = await query.ToListAsync();

        PaginatedResult<CategoryEntity> result = new()
        {
            ResultItems = data,
            RetrievedItems = data.Count,
            TotalAvailableItems = totalItemsFound,
        };
        return result;
    }

    
}
