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
        var endpoint = wa.MapPatch("/v{version:apiVersion}/products/{productId:guid}", HandleUpdateProduct);
        endpoint.Accepts<RR.Update.ProductRequest_V01>("application/json");
        endpoint.Produces<Ok<RR.Update.ProductResponse_V01>>(200,"application/json");
        endpoint.Produces<NotFound<string>>(404, "text/plain");
        endpoint.WithSummary("Updates a single product");
        endpoint.WithDescription("Updates a product with all the information provided");
        endpoint.WithDisplayName("Update Product");
        endpoint.WithName("product-update");
        endpoint.WithTags("Update Product");
        endpoint.MapToApiVersion(1, 0);
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