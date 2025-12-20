using FastEndpoints;

namespace CartService.UseCases.AddItemToCart.V01;

public class RequestDto
{
    //Route Params
    [RouteParam, BindFrom(AddItemToCart_V01.IdName)]
    public Guid CartId { get; set; }

    //Query Params
    [QueryParam, BindFrom(AddItemToCart_V01.ItemsVariable)]
    public List<int> ItemsToAdd { get; set; } = [];

    //Body Request
}
