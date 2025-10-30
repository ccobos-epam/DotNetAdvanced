using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;

namespace CatalogService.Product;

public class DeleteEndpoint : FE.Endpoint<FE.EmptyRequest, FE.EmptyResponse>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Delete(@"/products/{productId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid productId = Route<Guid>("productId");
        var response = await ProductService.DeleteProduct(productId);
        if (response)
            await Send.NoContentAsync(cancellationToken);
        else 
            await Send.NotFoundAsync(cancellationToken);
    }
}
