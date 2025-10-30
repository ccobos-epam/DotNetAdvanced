namespace CartService.Entities;
public class CartEntity
{
    public required Guid Id { get; set; }
    public required List<ItemEntity> ItemsInCart { get; set; } = [];
}