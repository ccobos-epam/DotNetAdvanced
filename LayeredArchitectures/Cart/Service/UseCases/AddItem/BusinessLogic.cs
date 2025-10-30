using CartService.Entities;

namespace CartService.UseCases.AddItem;

public interface IBusinessLogic
{
    Task AddItemToCart(Guid cartId, RequestDto request);
}

public class BusinessLogic : IBusinessLogic
{

    public IDataAccess DataAccess { get; set; }

    public async Task AddItemToCart(Guid cartId, RequestDto request)
    {
        var item = new ItemEntity
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
            Quantity = request.Quantity,
            Image = new ImageEntityPart
            {
                AltText = request.AltText,
                URLOfImage = request.URLOfImage,
            }
        };

        var cart = await DataAccess.GetCart(cartId);
        await DataAccess.AddItemToCart(cart, item);
    }
}
