using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product;

namespace CatalogService.Product;

public class GetEndpoint : FE.Endpoint<FE.EmptyRequest, GetProductResponse>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Get(@"/products/{productId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid productId = Route<Guid>("productId");
        var response = await ProductService.GetProduct(productId);
        await Send.OkAsync(response, cancellationToken);
    }
}
