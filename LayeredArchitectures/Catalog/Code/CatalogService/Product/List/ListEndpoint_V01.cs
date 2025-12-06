using FE = FastEndpoints;
using BusinessLayer;
using Utilities.Pagination;
using RR = BusinessLayer.Product.RR;
using BusinessLayer.Product.Service;

namespace CatalogService.Product.List;

//ToDo: Add enpoints support for exception or not found results
public class ListEndpoint_V01 : FE.Endpoint<RR.List.ProductRequest_V01, PaginatedResult<RR.List.ProductResponse_V01>>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Put("/products");
        AllowAnonymous();

        Version(1);

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<RR.List.ProductRequest_V01>("application/json");
            b.Produces<RR.List.ProductResponse_V01>(200, "application/json");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Retrieves multiples products given search and order criteria";
            s.Description = "Queries the database given filters so that we can fecth the data the client needs";
            s.Responses[200] = "a lists of products";
            object[] response200 = {
                new PaginatedResult<RR.List.ProductResponse_V01> {
                    PageNumber = 1 ,
                    PageSize = 1,
                    RetrievedItems = 1,
                    TotalAvailableItems = 2,
                    ResultItems = [
                        new RR.List.ProductResponse_V01 {
                            Id = Guid.CreateVersion7(),
                            Name = "Product Name",
                            ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE",
                            Ammount = 4,
                            CategoryId = Guid.NewGuid(),
                            CategoryName = "Category name",
                            Description = "Test Product",
                            Price = 45m
                        }
                    ]
                }
            };
            s.ResponseExamples[200] =  response200;
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(RR.List.ProductRequest_V01 request, CancellationToken cancellationToken)
    {
        var response = await ProductService.GetProductList(request.PagerValues!);
        await Send.OkAsync(response, cancellationToken);
    }
}
