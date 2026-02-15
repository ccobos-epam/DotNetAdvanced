namespace CartWebApi.Features.Cart.Entities;

public class Cart
{
  public required Guid  CartId { get; set; } = Guid.NewGuid();
  public List<Product> CartItems { get; set; } = [];
}