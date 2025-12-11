using Microsoft.AspNetCore.Http.HttpResults;
using FE = FastEndpoints;

namespace CartService.UseCases.AddItemToCart.V01;

public class AddItemToCart_V01 : FE.Endpoint<FE.EndpointWithoutRequest, Results<Ok, BadRequest<string>, NotFound<string>> >
{
    private const string IdName = "cartID";
    private const string ItemsVariable = "items";

    public IBusinessLogic BusinessLogic { get; set; }

    public override void Configure()
    {
        Put($"/carts/{{{IdName}}}");
        AllowAnonymous();
        Version(1);

        Action<RouteHandlerBuilder> rhb = b =>
        {
            b.Accepts<FE.EmptyRequest>("text/plain");
            b.Produces<Ok>(200, "application/json");
            b.Produces<BadRequest<string>>(400, "text/plain");
            b.Produces<NotFound<string>>(404, "text/plain");
        };
        Description(rhb, true);

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Add the items to a given cart";
            s.Description = "Given the item Ids, add all of them to a cart.";

            s.Params[IdName] = "The id of the cart to add items";
            s.Params[ItemsVariable] = "The list of Ids of items to add to the cart";


            s.ResponseExamples[200] = null!;
            s.ResponseExamples[400] = "The following item ids are not present: ";
            s.ResponseExamples[404] = $"The cart with the given ID: {Guid.NewGuid()} is not found.";
            s.Responses[200] = "The items were added to the cart";
            s.Responses[400] = "Some items were not found in the database";
            s.Responses[404] = "The cart was not found";
        };
        Summary(es);
    }

    public override Task<Results<Ok, BadRequest<string>, NotFound<string>>> ExecuteAsync(FE.EndpointWithoutRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
