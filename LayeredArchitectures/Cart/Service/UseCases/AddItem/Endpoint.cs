using FE = FastEndpoints;

namespace CartService.UseCases.AddItem;

public class Endpoint : FE.Endpoint<RequestDto, FE.EmptyResponse>
{
    private const string queryVariable = "cartId";
    private const string route = $$"""/cart/{{{queryVariable}}}""";

    public IBusinessLogic BusinessLayer { get; set; }

    public override void Configure()
    {
        Post(route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RequestDto request, CancellationToken ct)
    {
        Guid cartId = Route<Guid>(queryVariable);
        await BusinessLayer.AddItemToCart(cartId, request);
        await Send.OkAsync(ct);
    }
}
