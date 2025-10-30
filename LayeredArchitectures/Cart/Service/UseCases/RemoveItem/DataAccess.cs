using CartService.DataStorage;
using CartService.Entities;
using LiteDB;

namespace CartService.UseCases.RemoveItem;

public interface IDataAccess
{
    Task<CartEntity> GetCart(Guid cartId);
    Task DeleteItemFromCart(CartEntity cart, int itemIndex);
}
public class DataAccess : IDataAccess
{
    public IConfiguration Configuration { get; set; }

    public async Task DeleteItemFromCart(CartEntity cart, int itemIndex)
    {
        var connectionString = Configuration.GetConnectionString(InfrastructureData.connectionName);
        using var db = new LiteDatabase(connectionString);
        var col = db.GetCollection<CartEntity>(InfrastructureData.collectionName);

        cart.ItemsInCart.RemoveAt(itemIndex);
        col.Update(cart);
    }

    public async Task<CartEntity> GetCart(Guid cartId)
    {
        var connectionString = Configuration.GetConnectionString(InfrastructureData.connectionName);
        using var db = new LiteDatabase(connectionString);
        var col = db.GetCollection<CartEntity>(InfrastructureData.collectionName);

        var cart = col.Query()
            .Where(c => c.Id == cartId)
            .FirstOrDefault();

        return await Task.FromResult(cart);
    }
}
