using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category;

public class DeleteEndpoint : FE.Endpoint<FE.EmptyRequest, FE.EmptyResponse>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Delete(@"/category/{categoryId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid categoryId = Route<Guid>("categoryId");
        var response = await CategoryService.DeleteCategory(categoryId);
        if (response)
            await Send.NoContentAsync(cancellationToken);
        else 
            await Send.NotFoundAsync(cancellationToken);
    }
}
