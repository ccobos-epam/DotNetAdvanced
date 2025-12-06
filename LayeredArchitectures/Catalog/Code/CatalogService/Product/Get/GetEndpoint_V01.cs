using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer.Product.Service;
using RR = BusinessLayer.Product.RR;

namespace CatalogService.Product.Get;

public class GetEndpoint_V01 : FE.Endpoint<FE.EmptyRequest, RR.Get.ProductResponse_V01>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Get(@"/products/{productId}");
        AllowAnonymous();

        Version(1);
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<FE.EmptyRequest>("application/json");
            b.Produces<RR.Get.ProductResponse_V01>(200, "application/json");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Retrieves a single product";
            s.Description = "Query for a product given an Id";
            s.Responses[200] = "Individual Product";
            object response200 = new RR.Get.ProductResponse_V01
            {
                Id = Guid.CreateVersion7(),
                Name = "Product Name",
                ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE",
                Ammount = 4,
                CategoryId = Guid.NewGuid(),
                CategoryName = "Category name",
                Description = "Test Product",
                Price = 45m
            };
            s.ResponseExamples[200] =  response200;
            s.Params["productId"] = "The id of the product to search";
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken cancellationToken)
    {
        Guid productId = Route<Guid>("productId");
        var response = await ProductService.GetProduct(productId);
        await Send.OkAsync(response, cancellationToken);
    }
}
