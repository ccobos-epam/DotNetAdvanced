using Microsoft.AspNetCore.Http.HttpResults;
using FE = FastEndpoints;

namespace CartService.UseCases.GetCart.V02;

public class GetEndpoint_V02 : FE.Endpoint<FE.EmptyRequest, Results<Ok<List<ResponseDto>>, NotFound<string>> >
{
    private const string IdName = "cartID";
    public IBusinessLogic BusinessLogic { get; set; }

    public override void Configure()
    {
        Get($"/carts/{{{IdName}}}");
        AllowAnonymous();
        Version(2);

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<FE.EmptyRequest>("application/json");
            b.Produces<ResponseDto>(200, "application/json");
            b.Produces<NotFound<string>>(404, "text/plain");
        };

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Query a cart to obtain their items";
            s.Description = "Call the respective cart to obtain the items inside them";

            s.Params[IdName] = "The Id of the cart to search";

            object[] response200 = {
                new ResponseDto
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ItemName = "item1",
                        ItemQuantity = "description1",
                        ItemPrice = (Convert.ToDecimal(Random.Shared.NextDouble()) * 50m).ToString()
                    },
                new ResponseDto
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ItemName = "item2",
                        ItemQuantity = "description2",
                        ItemPrice = (Convert.ToDecimal(Random.Shared.NextDouble()) * 50m).ToString()
                    }
            };
            object response404 = Guid.NewGuid().ToString();
            s.ResponseExamples[200] = response200;
            s.ResponseExamples[404] = response404;
            s.Responses[200] = "The items for the cart";
            s.Responses[404] = "The cart id that failed";

        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task<Results< Ok<List<ResponseDto>> , NotFound<string> >> 
        ExecuteAsync(FE.EmptyRequest emtpyRequest, CancellationToken cancellationToken)
    {
        Guid cartId = Route<Guid>(IdName);
        var result = await BusinessLogic.GetItemsInCart(cartId);
        var httpResult = result.Match(found, notFound);
        return httpResult;

        Results<Ok<List<ResponseDto>>, NotFound<string>> found(List<ResponseDto> result) 
            => TypedResults.Ok(result);
        Results<Ok<List<ResponseDto>>, NotFound<string>> notFound(OneOf.Types.NotFound result) 
            => TypedResults.NotFound<string>($"No cart was found with Id: {cartId}");
    }
}
