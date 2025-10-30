namespace CartService.Entities;
public class ItemEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public ImageEntityPart? Image { get; set; }
}

public class ImageEntityPart
{
    public string URLOfImage { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
}