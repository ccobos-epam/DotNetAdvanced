using FE = FastEndpoints;

namespace CartService.UseCases.CreateCart;

public class Endpoint : FE.Endpoint<FE.EmptyRequest,Guid>
{
    private const string route = "/cart";

    public IBusinessLogic BusinessLayer { get; set; }

    public override void Configure()
    {
        Post(route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken ct)
    {
        var result = await BusinessLayer.CreateNewCart();
        await Send.OkAsync(result, cancellation: ct);
    }
}
