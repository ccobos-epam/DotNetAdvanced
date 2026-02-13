using Asp.Versioning;
using CartWebApi.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace CartWebApi.Features.Cart.CreateCart.V01;

public class Endpoint : IEndpointDefinition
{
  private const string BaseRoute = "api/v{apiVersion:apiVersion}/carts";

  public void DefineServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<Handler>();
  }
  
  public void DefineEndpoints(IEndpointRouteBuilder endpoints)
  {
    var apiVersionSet = endpoints.NewApiVersionSet()
      .HasApiVersion(new ApiVersion(1,0))
      .ReportApiVersions()
      .Build();
    
    var group = endpoints.MapGroup(BaseRoute).WithApiVersionSet(apiVersionSet);

    group.MapPost("/", HandleRouteV01)
      .WithName("CreateCart-V01");
  }

  private static async Task<Results<Created<string>, InternalServerError<string>>> HandleRouteV01(
    [FromServices] Handler handler)
  {
    var newCartGuid = Guid.CreateVersion7();
    OneOf<Success, Error> result = await handler.CreateCart(newCartGuid);
    return result.Match<Results<Created<string>, InternalServerError<string>>>(
      _ => TypedResults.Created(string.Empty, newCartGuid.ToString()),
      _ => TypedResults.InternalServerError("Error")
    );
  }
}