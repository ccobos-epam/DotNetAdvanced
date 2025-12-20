using OneOf;
using OneOf.Types;

namespace CartService.UseCases.AddItemToCart.V01;

public interface IBusinessLogic
{
    Task<OneOf<Success, Success<int[]>, Error<int[]>, NotFound, Error>>
        AddItemsToCart(Guid cartId, params int[] itemIds);
}
