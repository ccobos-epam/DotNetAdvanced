using OFT = OneOf.Types;
using OneOf;

namespace CartService.UseCases.GetCart.V01;

public interface IBusinessLogic
{
    Task<OneOf<ResponseDto, OFT.NotFound>> GetCartWithItems(Guid cartId);
}
