using BusinessLayer.Category;
using DataAccess;
using DomainEntities;

namespace BusinessLayer;

public interface ICategoryService
{
    Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request);
    Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request);
    Task<GetCategoryResponse> GetCategory(Guid categoryId);
    Task<IList<GetCategoryResponse>> GetCategoryList();
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

    public async Task<IList<GetCategoryResponse>> GetCategoryList()
    {
        var entityList = await repository.List();
        var response = entityList.Select(x => new GetCategoryResponse { 
            Id = x.Id,
            Name = x.Name,
            ImageUrl = x.ImageUrl,
            ParentCategoryId = x.ParentCategoryId,
            ParentCategoryName = x.ParentCategory?.Name,
        }).ToList();
        return response;
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
}
