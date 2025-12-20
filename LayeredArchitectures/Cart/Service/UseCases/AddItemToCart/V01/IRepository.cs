using CartService.Entities;
using OneOf;

namespace CartService.UseCases.AddItemToCart.V01;

public interface IRepository
{
    Task<CartEntity?> GetCart(Guid cartID);

    Task<(IList<ItemEntity> validItems, int[] errorItems)> GetItems(params int[] itemIds);

    Task AddItemsToCart(CartEntity cart, IList<ItemEntity> items);
}
