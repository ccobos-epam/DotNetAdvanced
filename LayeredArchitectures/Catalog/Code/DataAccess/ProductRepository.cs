using DomainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Utilities.Pagination;

namespace DataAccess;

public interface IProductRepository
{
    Task<ProductEntity?> GetSingle(Guid id);
    Task<PaginatedResult<ProductEntity>> List(
        IList<Expression<Func<ProductEntity, bool>>> filteringConditions,
        Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>> sortingConditions,
        Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>> paginationCondition);
    Task AddSingle(ProductEntity entity);
    Task UpdateSingle(ProductEntity entity);
    Task<bool> DeleteSingle(Guid id);
}

public class ProductRepository : IProductRepository
{

    private readonly AppDbContext _appDbContext;

    public ProductRepository(
        AppDbContext appDbContext
        )
    {
        _appDbContext = appDbContext;
    }

    public async Task AddSingle(ProductEntity entity)
    {
        _appDbContext.Products.Add(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteSingle(Guid id)
    {
        var entityToDelete = _appDbContext.Products.FirstOrDefault(x => x.Id == id);
        if (entityToDelete == null)
            return await Task.FromResult(false);
        _appDbContext.Remove(entityToDelete);
        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<ProductEntity?> GetSingle(Guid id)
    {
        var entityToReturn = await _appDbContext.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        return entityToReturn;
    }

    public async Task<PaginatedResult<ProductEntity>> List(
        IList<Expression<Func<ProductEntity, bool>>> filteringConditions,
        Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>> sortingConditions,
        Func<IQueryable<ProductEntity>, IQueryable<ProductEntity>> paginationCondition)
    {
        var baseQuey = _appDbContext.Products.Include(x => x.Category).AsSplitQuery();

        foreach (var item in filteringConditions)
            baseQuey = baseQuey.Where(item);

        int totalItemsFound = await baseQuey.CountAsync();

        baseQuey = sortingConditions(baseQuey);
        baseQuey = paginationCondition(baseQuey);

        var data = await baseQuey.ToListAsync();

        PaginatedResult<ProductEntity> result = new()
        {
            ResultItems = data,
            RetrievedItems = data.Count,
            TotalAvailableItems = totalItemsFound,
        };
        return result;
    }

    public async Task UpdateSingle(ProductEntity entity)
    {
        await _appDbContext.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }
}
