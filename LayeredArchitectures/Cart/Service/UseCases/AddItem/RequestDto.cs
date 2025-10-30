namespace CartService.UseCases.AddItem;

public record RequestDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public string URLOfImage { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
}
