using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CartService.UseCases.GetCart.V01;

public record ItemDto
{
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemPrice { get; set; }
    public string ItemQuantity { get; set; }
}
