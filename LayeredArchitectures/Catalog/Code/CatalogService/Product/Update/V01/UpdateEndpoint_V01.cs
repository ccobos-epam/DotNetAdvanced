using BusinessLayer;
using RR = BusinessLayer.Product.RR;
using BusinessLayer.Product.Service;
using FE = FastEndpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DataAccess;


using ReturnType = System.Threading.Tasks.Task<
    Microsoft.AspNetCore.Http.HttpResults.Results<
        Microsoft.AspNetCore.Http.HttpResults.Ok<BusinessLayer.Product.RR.Update.ProductResponse_V01>,
        Microsoft.AspNetCore.Http.HttpResults.NotFound<string>
        >
    >;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;


namespace CatalogService.Product.Update.V01;

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

public static class UpdateHandler
{
    public static IServiceCollection AddUpdateServices(IServiceCollection services)
    {
        services.TryAddScoped<IProductRepository, ProductRepository>();
        services.TryAddScoped<IProductService, ProductService>();
        return services;
    }

    public static WebApplication RegisterUpdateRoutes(WebApplication wa)
    {
        var endpoint = wa.MapPatch("/products/{productId:guid}", HandleUpdateProduct);
        endpoint.Accepts<RR.Update.ProductRequest_V01>("application/json");
        endpoint.Produces<Ok<RR.Update.ProductResponse_V01>>(200,"application/json");
        endpoint.Produces<NotFound<string>>(404, "text/plain");
        endpoint.WithSummary("Updates a single product");
        endpoint.WithDescription("Updates a product with all the information provided");
        endpoint.WithDisplayName("Update Product");
        endpoint.WithName("product-update");
        endpoint.AllowAnonymous();
        return wa;
    }

    public static async ReturnType HandleUpdateProduct(
        [FromRoute(Name = "productId")] Guid singleProductId,
        [FromBody] RR.Update.ProductRequest_V01 inputRequest,
        [FromServices] IProductService productService)
    {
        RR.Update.ProductRequest_V01 newRequest = inputRequest with { Id = singleProductId };
        var serviceTask = productService.UpdateProduct(newRequest);
        var result = await serviceTask;
        if (serviceTask.IsCompletedSuccessfully)
        {
            return TypedResults.Ok(result);
        }
        else
        {
            return TypedResults.NotFound($"The product with Id {singleProductId} was not found");
        }
    }
}