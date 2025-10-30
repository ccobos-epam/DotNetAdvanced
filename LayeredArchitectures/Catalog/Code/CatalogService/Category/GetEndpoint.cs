using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category;

public class GetEndpoint : FE.Endpoint<FE.EmptyRequest, GetCategoryResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Get(@"/category/{categoryId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid categoryId = Route<Guid>("categoryId");
        var response = await CategoryService.GetCategory(categoryId);
        await Send.OkAsync(response);
    }
}
