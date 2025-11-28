using FE = FastEndpoints;
using BusinessLayer.Category;
using BusinessLayer;
using BusinessLayer.Product;

namespace CatalogService.Product.Create;

public class EndpointV01 : FE.Endpoint<CreateProductRequest, CreateProductResponse>
{
    public IProductService ProductService { get; set; }

    public override void Configure()
    {
        Post("/products");
        Version(1);
        AllowAnonymous();

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<CreateProductRequest>("application/json+custom");
            b.Produces<CreateProductResponse>(201, "application/json+custom");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Create a single product";
            s.Description = "Create a new product a return the product mapping to the caller";
            s.RequestExamples
                .Add(new FE.RequestExample(
                    new CreateProductRequest
                    {
                        Name = "New Product Name",
                        CategoryId = Guid.NewGuid(),
                        Ammount = 10,
                        Description = "Test product",
                        Price = 45m,
                        ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE"
                    },
                    "Simple product creation example",
                    "New category creation",
                    "Send the values ")
                );
            s.ResponseExamples[201] = new CreateProductResponse
            {
                Id = Guid.NewGuid(),
                Name = "New Product Name",
                ImageUrl = @"https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fes%2Ffotos%2Famor&psig=AOvVaw0CUb7IMmNazRRlJNHUbZd2&ust=1763487208730000&source=images&cd=vfe&opi=89978449&ved=0CBUQjRxqFwoTCNifhPXb-ZADFQAAAAAdAAAAABAE",
                CategoryId = Guid.NewGuid(),
                CategoryName = "Category Name",
                Ammount = 10,
                Description = "Test product",
                Price = 45m
            };
            s.Responses[201] = "The newly created object";
        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task HandleAsync(CreateProductRequest request,  CancellationToken cancellationToken)
    {
        var response = await ProductService.CreateProduct(request);
        await Send.CreatedAtAsync<GetEndpoint>(response.Id,response, FE.Http.GET, null, false, default);
    }
}
