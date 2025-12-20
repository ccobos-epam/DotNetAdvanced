namespace CartService.UseCases.AddItemToCart.V01;

public record ResponseDto
{
    public int[] CartItemsNotFound { get; set; } = [];
    public int NumberOfItemsNotFound { get; set; } = 0;
    public Guid CartId { get; set; }
}
