using OneOf;
using OneOf.Types;

namespace CartService.UseCases.AddItemToCart.V01;

public class BusinessLogic : IBusinessLogic
{
    public IRepository Repository { get; set; }

    public async Task<OneOf<Success, Success<int[]>, Error<int[]>, NotFound, Error>> AddItemsToCart(Guid cartId, params int[] itemIds)
    {
        var cartEntity = await Repository.GetCart(cartId);
        if (cartEntity is null)
            return new OneOf.Types.NotFound();

        var (validItems, errorItems)= await Repository.GetItems(itemIds);
        if(validItems.Count ==0)
            return new Error<int[]>(errorItems);

        var addTask = Repository.AddItemsToCart(cartEntity!, validItems);
        await addTask;

        if (!addTask.IsCompletedSuccessfully)
            return new Error();

        if (errorItems.Length != 0)
            return new Success<int[]>(errorItems);
        else
            return new Success();
    }
}
