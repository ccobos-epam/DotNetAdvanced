using CartService.UseCases.AddItem;

namespace CartService.UseCases.RemoveItem;

public interface IBusinessLogic
{
    Task RemoveItemFromCart(Guid cartId, int itemId);
}
public class BusinessLogic : IBusinessLogic
{
    public IDataAccess DataAccess { get; set; }

    public async Task RemoveItemFromCart(Guid cartId, int itemId)
    {
        var cart = await DataAccess.GetCart(cartId);
        var index = cart.ItemsInCart.Where(item => item.Id == itemId).Select((ie, i) => i).FirstOrDefault();
        await DataAccess.DeleteItemFromCart(cart, index);
    }
}
