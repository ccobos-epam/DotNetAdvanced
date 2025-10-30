using FE = FastEndpoints;

namespace CartService.UseCases.GetList;

public class Endpoint : FE.Endpoint<FE.EmptyRequest, ResponseDto>
{
    private const string queryVariable = "cartId";
    private const string route = $$"""/cart/{{{queryVariable}}}""";

    public IBusinessLogic BusinessLayer { get; set; }
    public override void Configure()
    {
        Get(route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(FE.EmptyRequest request, CancellationToken ct)
    {
        Guid cartId = Route<Guid>(queryVariable);
        var result = await BusinessLayer.GetListOfItemsinCart(cartId);
        await Send.OkAsync(result, cancellation: ct);
    }
}
