using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product.List;

namespace CatalogService.Category;

public class ListEndpoint : FE.Endpoint<ListProductRequest, IList<GetCategoryResponse>>
{
    public ICategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Get(@"/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListProductRequest request, CancellationToken cancellationToken)
    {
        var response = await CategoryService.GetCategoryList();
        await Send.OkAsync(response, cancellationToken);
    }
}
