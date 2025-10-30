namespace CartService.UseCases.CreateCart;

public interface IBusinessLogic
{
    Task<Guid> CreateNewCart();
}

public class BusinessLogic : IBusinessLogic
{
    public IDataAccess DataAccess { get; set; }

    public async Task<Guid> CreateNewCart()
    {
        Guid id = Guid.CreateVersion7();

        var newCart = await DataAccess.CreateNewCart(id);

        return await Task.FromResult(newCart.Id);
    }
}
