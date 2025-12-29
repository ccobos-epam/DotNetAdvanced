using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using FE = FastEndpoints;

namespace CartService.UseCases.AddItemToCart.V01;

public class AddItemToCart_V01 : FE.Endpoint<RequestDto, ResponseDto>
{
    public const string IdName = "cartID";
    public const string ItemsVariable = "items";

    public IBusinessLogic BusinessLogic { get; set; }

    public override void Configure()
    {
        Put("/carts/{cartId}");
        AllowAnonymous();
        Version(1);

        Action<RouteHandlerBuilder> rhb = b =>
        {
            //b.Accepts(null, isOptional: true, "text/plain", "application/json");
            b.Produces<Ok>(200, "application/json");
            b.Produces<BadRequest<string>>(400, "text/plain");
            b.Produces<NotFound<string>>(404, "text/plain");
            b.Produces<InternalServerError>(500, "text/plain");
        };
        Description(rhb, true);

        Action<FE.EndpointSummary> es = s =>
        {
            s.Summary = "Add the items to a given cart";
            s.Description = "Given the item Ids, add all of them to a cart.";

            s.Params[IdName] = "The id of the cart to add items";
            s.Params[ItemsVariable] = "The list of Ids of items to add to the cart";

            s.ResponseExamples[200] = null!;
            s.ResponseExamples[400] = "The following item ids are not present: 3, 4, 5";
            s.ResponseExamples[404] = $"The cart with the given ID: {Guid.NewGuid()} is not found.";
            s.Responses[200] = "The items were added to the cart";
            s.Responses[400] = "Some items were not found in the database";
            s.Responses[404] = "The cart was not found";
            s.Responses[500] = "An Internal error happened";
        };
        Summary(es);
    }

    public override async Task<FE.Void> HandleAsync(RequestDto req, CancellationToken ct)
    {
        var logicRespose = await BusinessLogic.AddItemsToCart(req.CartId, [.. req.ItemsToAdd]);
        throw new NotImplementedException();
        //return await logicRespose.Match(
        //    async completeSuccess => await Send.ResultAsync(TypedResults.Ok<ResponseDto>( new ResponseDto { CartId = req.CartId })),
        //    partialSuccess => Send.ResultAsync(TypedResults.Ok<ResponseDto>( new ResponseDto { 
        //        CartId = req.CartId, 
        //        CartItemsNotFound = partialSuccess.Value, 
        //        NumberOfItemsNotFound = partialSuccess.Value.Length})),
        //    itemsNotFound => this.Send.ResultAsync(TypedResults.BadRequest<string>("The following item ids are not present: " + String.Join(", ", itemsNotFound))),
        //    _ => Send.ResultAsync(TypedResults.NotFound<string>($"The cart with the given ID: {req.CartId} is not found.")),
        //    _ => Send.ResultAsync(TypedResults.InternalServerError())
        //    );


    }
}
