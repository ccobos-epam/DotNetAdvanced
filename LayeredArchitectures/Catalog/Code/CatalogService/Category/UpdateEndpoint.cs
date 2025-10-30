using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category;

public class UpdateEnpoint : FE.Endpoint<UpdateCategoryRequest, UpdateCategoryResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Patch("/category/{categoryId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCategoryRequest request,  CancellationToken cancellationToken)
    {
        Guid categoryId = Route<Guid>("categoryId");
        UpdateCategoryRequest newRequest = request with { CategoryId = categoryId };
        var response = await CategoryService.UpdateCategory(newRequest);
        await Send.OkAsync(response);
    }
}
