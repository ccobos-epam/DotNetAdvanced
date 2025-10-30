using BusinessLayer;
using BusinessLayer.Product;
using FE = FastEndpoints;

namespace CatalogService.Product;

public class UpdateEnpoint : FE.Endpoint<UpdateProductRequest, UpdateProductResponse>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Patch("/products/{productId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateProductRequest request,  CancellationToken cancellationToken)
    {
        Guid productId = Route<Guid>("productId");
        UpdateProductRequest newRequest = request with { Id = productId };
        var response = await ProductService.UpdateProduct(newRequest);
        await Send.OkAsync(response, cancellationToken);
    }
}
