namespace CartService.UseCases.GetCart.V01;

public record ResponseDto
{
    public string CartId { get; set; } = string.Empty;

    public List<ItemDto> Items { get; set; } = Enumerable.Empty<ItemDto>().ToList();
}
