namespace CartWebApi.Features.Cart.Entities;

public class Product
{
  public required Guid Id { get; set; }
  public required string Name { get; set; }
  public string? ImageUrl { get; set; }
  public string? ImageAltText { get; set; }
  public required decimal Price { get; set; }
  public required int Quantity { get; set; } = 1;
}