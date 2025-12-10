using Microsoft.AspNetCore.Http.HttpResults;
using FE = FastEndpoints;

namespace CartService.UseCases.GetCart.V01;

public class GetEndpoint_V01 : FE.Endpoint<FE.EmptyRequest, Results<Ok<ResponseDto>, NotFound<string>> >
{
    private const string IdName = "cartID";
    public IBusinessLogic BusinessLogic { get; set; }

    public override void Configure()
    {
        Get($"/carts/{{{IdName}}}");
        AllowAnonymous();
        Version(1);

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

            object response200 = new ResponseDto
            {
                CartId = Guid.NewGuid().ToString(),
                Items = {
                    new ItemDto 
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ItemName = "item1",
                        ItemQuantity = "description1",
                        ItemPrice = (Convert.ToDecimal(Random.Shared.NextDouble()) * 50m).ToString()
                    },
                    new ItemDto
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ItemName = "item2",
                        ItemQuantity = "description2",
                        ItemPrice = (Convert.ToDecimal(Random.Shared.NextDouble()) * 50m).ToString()
                    }
                }
            };
            object response404 = Guid.NewGuid().ToString();
            s.ResponseExamples[200] = response200;
            s.ResponseExamples[404] = response404;
            s.Responses[200] = "The obtained cart with items";
            s.Responses[404] = "The cart id that failed";

        };

        Description(rhb, true);
        Summary(es);
    }

    public override async Task<Results< Ok<ResponseDto> , NotFound<string> >> 
        ExecuteAsync(FE.EmptyRequest emtpyRequest, CancellationToken cancellationToken)
    {
        /*
        Guid cartId = Route<Guid>(IdName);
        var result = await BusinessLogic.GetCartWithItems(cartId);
        Func<ResponseDto, Results<Ok<ResponseDto>, NotFound<string>>> found = result => TypedResults.Ok<ResponseDto>(result);
        Func<OneOf.Types.NotFound, Results<Ok<ResponseDto>, NotFound<string>>> notFound = result => TypedResults.NotFound<string>($"No cart was found with Id: {cartId}");
        var httpResult = result.Match<Results<Ok<ResponseDto>, NotFound<string>>>(
            found => TypedResults.Ok<ResponseDto>(found), 
            notFound => TypedResults.NotFound<string>($"No cart was found with Id: {cartId}"));
        return httpResult;
        */
        Guid cartId = Route<Guid>(IdName);
        var result = await BusinessLogic.GetCartWithItems(cartId);
        var httpResult = result.Match(found, notFound);
        return httpResult;

        Results<Ok<ResponseDto>, NotFound<string>> found(ResponseDto result) 
            => TypedResults.Ok<ResponseDto>(result);
        Results<Ok<ResponseDto>, NotFound<string>> notFound(OneOf.Types.NotFound result) 
            => TypedResults.NotFound<string>($"No cart was found with Id: {cartId}");
    }
}
