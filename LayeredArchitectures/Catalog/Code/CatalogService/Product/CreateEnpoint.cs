using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product;

namespace CatalogService.Product;

public class CreateEnpoint : FE.Endpoint<CreateProductRequest, CreateProductResponse>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest request,  CancellationToken cancellationToken)
    {
        var response = await ProductService.CreateProduct(request);
        await Send.CreatedAtAsync<GetEndpoint>(response.Id,response, FE.Http.GET, null, false, default);
    }
}
