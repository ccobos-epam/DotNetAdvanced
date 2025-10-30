using DomainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess;

public interface IProductRepository
{
    Task<ProductEntity?> GetSingle(Guid id);
    Task<IList<ProductEntity>> List();
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

    public async Task<IList<ProductEntity>> List()
    {
        var collection = await _appDbContext.Products.Include(x => x.Category).AsSplitQuery().ToListAsync();
        return collection;
    }

    public async Task UpdateSingle(ProductEntity entity)
    {
        await _appDbContext.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }
}
