using OneOf;
using OneOf.Types;
using V01 = CartService.UseCases.GetCart.V01;

namespace CartService.UseCases.GetCart.V02;

public class BusinessLogic : IBusinessLogic
{
    #region Dependency Injection Properties
    public V01.IRepository Repository { get; set; }
    #endregion

    public async Task<OneOf<List<ResponseDto>, NotFound>> GetItemsInCart(Guid cartId)
    {
        var result = await Repository.GetById(cartId);
        
        if(result is null)
            return new NotFound();

        var mappedResult = result.ItemsInCart.Select(x => new ResponseDto {
            ItemId = x.Id.ToString(),
            ItemName    = x.Name,
            ItemPrice = x.Price.ToString(),
            ItemQuantity = x?.ToString()
        }).ToList();

        return mappedResult;
    }
}
