using BusinessLayer;
using RR = BusinessLayer.Product.RR;
using BusinessLayer.Product.Service;
using FE = FastEndpoints;

namespace CatalogService.Product.Update;

public class UpdateEndpoint_V01 : FE.Endpoint<RR.Update.ProductRequest_V01, RR.Update.ProductResponse_V01>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Patch("/products/{productId}");
        Version(1);
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<RR.List.ProductRequest_V01>("application/json");
            b.Produces<RR.List.ProductResponse_V01>(200, "application/json");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Updates a single product";
            s.Description = "Updates a product with all the information provided";
            s.Responses[200] = "the updated product";
            object response200 = new RR.Update.ProductResponse_V01() 
            {
                Id = Guid.NewGuid(),
                Name = "Updated product",
                Description = "Updated Description",
                Price = 10,
                Ammount = 3,
                ImageUrl = "testUrl",
                CategoryName = "Category",
                CategoryId = Guid.NewGuid(),
            };
            s.ResponseExamples[200] =  response200;
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(RR.Update.ProductRequest_V01 request,  CancellationToken cancellationToken)
    {
        Guid productId = Route<Guid>("productId");
        RR.Update.ProductRequest_V01 newRequest = request with { Id = productId };
        var response = await ProductService.UpdateProduct(newRequest);
        await Send.OkAsync(response, cancellationToken);
    }
}
