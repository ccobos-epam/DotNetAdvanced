using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product.List;

namespace CatalogService.Category;

public class ListEndpoint : FE.Endpoint<ListProductRequest, IList<GetCategoryResponse>>
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public ICategoryService CategoryService { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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
