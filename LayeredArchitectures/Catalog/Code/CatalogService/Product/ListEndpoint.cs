using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product;

namespace CatalogService.Product;

public class ListEndpoint : FE.Endpoint<FE.EmptyRequest, IList<GetProductResponse>>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        var response = await ProductService.GetProductList(new BusinessLayer.Pager.PagerObject());
        await Send.OkAsync(response, cancellationToken);
    }
}
