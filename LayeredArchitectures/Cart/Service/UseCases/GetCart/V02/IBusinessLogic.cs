using OFT = OneOf.Types;
using OneOf;

namespace CartService.UseCases.GetCart.V02;

public interface IBusinessLogic
{
    Task<OneOf<List<ResponseDto>, OFT.NotFound>> GetItemsInCart(Guid cartId);
}
