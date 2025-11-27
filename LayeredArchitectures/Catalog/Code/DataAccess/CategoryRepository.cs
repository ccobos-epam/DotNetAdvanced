using DomainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess;

public interface ICategoryRepository 
{
    Task<CategoryEntity?> GetSingle(Guid id);
    Task<IList<CategoryEntity>> List();
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

    public async Task<IList<CategoryEntity>> List()
    {
        var dbSet = _appDbContext.Categories;
        var collection = await dbSet.Include(x => x.ParentCategory).ToListAsync();
        return collection;
    }

    public async Task UpdateSingle(CategoryEntity entity)
    {
        _appDbContext.Categories.Update(entity);
        await _appDbContext.SaveChangesAsync(); 
    }
}
