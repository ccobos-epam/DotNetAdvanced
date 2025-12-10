using OneOf;
using OneOf.Types;

namespace CartService.UseCases.GetCart.V01;

public class BusinessLogic : IBusinessLogic
{
    #region Dependency Injection Properties
    public IRepository Repository { get; set; }
    #endregion

    public async Task<OneOf<ResponseDto, NotFound>> GetCartWithItems(Guid cartId)
    {
        var result = await Repository.GetById(cartId);
        
        if(result is null)
            return new NotFound();

        var mappedResult = new ResponseDto
        {
            CartId = result.Id.ToString(),
            Items = [.. result.ItemsInCart.Select(x => new ItemDto
            {
                ItemId = x.Id.ToString(),
                ItemName    = x.Name,
                ItemPrice = x.Price.ToString(),
                ItemQuantity = x?.ToString()
            })],
        };
        return mappedResult;
    }
}
