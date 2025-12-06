using BusinessLayer.Category.RR;
using DataAccess;
using DomainEntities;
using System.Linq.Expressions;
using Utilities.Pagination;

namespace BusinessLayer.Category;

public interface ICategoryService
{
    Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request);
    Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request);
    Task<GetCategoryResponse> GetCategory(Guid categoryId);
    Task<PaginatedResult<GetCategoryResponse>> GetCategoryList(PagerObject pagerObject);
    Task<bool> DeleteCategory(Guid categoryId);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository repository;

    public CategoryService(ICategoryRepository repository)
    {
        this.repository = repository;
    }

    public async Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request)
    {
        CategoryEntity newCategory = new CategoryEntity()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            ImageUrl = request.ImageUrl,
            ParentCategoryId = request?.ParentCategoryId,
        };
        await repository.AddSingle(newCategory);
        var createdCategory = await repository.GetSingle(newCategory.Id);
        CreateCategoryResponse response = new CreateCategoryResponse
        {
            Id = createdCategory!.Id,
            Name = createdCategory.Name,
            ImageUrl = createdCategory.ImageUrl,
            ParentCategoryId= createdCategory.ParentCategoryId,
            ParentCategoryName = createdCategory.ParentCategory?.Name,
        };
        return response;
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        return await repository.DeleteSingle(categoryId);
    }

    public async Task<GetCategoryResponse> GetCategory(Guid categoryId)
    {
        var category = await repository.GetSingle(categoryId);
        var result = new GetCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = category.ParentCategory?.Name,
        };
        return result;
    }

    public async Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request)
    {
        var categoryToModify = await repository.GetSingle(request.CategoryId);
        categoryToModify.ParentCategoryId = (request.ParentCategoryId.HasValue) ? request.ParentCategoryId.Value : null;
        categoryToModify.ImageUrl = request.ImageUrl;
        categoryToModify.Name = request.Name;
        categoryToModify.ImageUrl = request.ImageUrl;
        await repository.UpdateSingle(categoryToModify);
        var mofiedCategory = await repository.GetSingle(request.CategoryId);
        var response = new UpdateCategoryResponse()
        {
            Id = mofiedCategory.Id,
            Name = mofiedCategory.Name,
            ImageUrl = mofiedCategory.ImageUrl,
            ParentCategoryId= mofiedCategory.ParentCategoryId,
            ParentCategoryName = mofiedCategory.ParentCategory?.Name,
        };
        return response;
    }

    public async Task<PaginatedResult<GetCategoryResponse>> GetCategoryList(PagerObject pagerObject)
    {
        List<Expression<Func<CategoryEntity, bool>>> filteringConditions = CategoryFiltering.SelectFilteringConditions(pagerObject.FilteringValues);
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> sortingConditions = CategoryOrdering.CreateOrderingStrategy(pagerObject.SortingValues);
        Func<IQueryable<CategoryEntity>, IQueryable<CategoryEntity>> paginationCondition = GeneralPagination.CreatePagination<CategoryEntity>(pagerObject.PaginationValues);
        var entityList = await repository.List(filteringConditions, sortingConditions, paginationCondition);

        PaginatedResult<GetCategoryResponse> response = new()
        {
            PageNumber = pagerObject.PaginationValues.PageIndex,
            PageSize = pagerObject.PaginationValues.RecordsPerPage,
            RetrievedItems = entityList.RetrievedItems,
            TotalAvailableItems = entityList.TotalAvailableItems,
            ResultItems = entityList.ResultItems.Select(x => new GetCategoryResponse
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                ParentCategoryId = x.ParentCategoryId,
                ParentCategoryName = x.ParentCategory?.Name,
            }).ToList(),
        };
            
        return response;
    }

    

}
