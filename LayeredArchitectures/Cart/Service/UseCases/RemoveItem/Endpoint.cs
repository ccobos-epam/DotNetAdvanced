using FE = FastEndpoints;

namespace CartService.UseCases.RemoveItem;

public class Endpoint : FE.Endpoint<FE.EmptyRequest, FE.EmptyResponse>
{
    private const string cartVariable = "cartId";
    private const string itemVariable = "itemId";
    private const string route = $$"""/cart/{{{cartVariable}}}/{{{itemVariable}}}""";

    public IBusinessLogic BusinessLayer { get; set; }

    public override void Configure()
    {
        Delete(route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken ct)
    {
        Guid cartId = Route<Guid>(cartVariable);
        int itemId = Route<int>(itemVariable);
        await BusinessLayer.RemoveItemFromCart(cartId, itemId);
        await Send.OkAsync(ct);
    }
}
