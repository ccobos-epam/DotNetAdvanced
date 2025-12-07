using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product.Service;

namespace CatalogService.Product.Delete;

public class DeleteEndpoint : FE.Endpoint<FE.EmptyRequest, FE.EmptyResponse>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Delete(@"/products/{productId}");
        Version(1);
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<FE.EmptyRequest>("application/json+custom");
            b.Produces<FE.EmptyResponse>(204, "application/json+custom");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Deletes a single product";
            s.Description = "Deletes a product from the database given an Id";
            s.Responses[204] = "An acceptance of the deleted object";
            s.Params["productId"] = "The id of the product to delete";
        };

        Description(rhb, true);
        Summary(es);
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
