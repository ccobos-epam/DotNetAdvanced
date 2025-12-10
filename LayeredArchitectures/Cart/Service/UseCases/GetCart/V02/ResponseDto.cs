using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CartService.UseCases.GetCart.V02;

public record ResponseDto
{
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemPrice { get; set; }
    public string ItemQuantity { get; set; }
}
