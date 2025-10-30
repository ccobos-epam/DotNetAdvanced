using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Category;

public class ListEndpoint : FE.Endpoint<FE.EmptyRequest, IList<GetCategoryResponse>>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Get(@"/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        var response = await CategoryService.GetCategoryList();
        await Send.OkAsync(response, cancellationToken);
    }
}
