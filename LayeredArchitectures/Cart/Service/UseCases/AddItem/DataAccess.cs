using CartService.DataStorage;
using CartService.Entities;
using LiteDB;

namespace CartService.UseCases.AddItem;

public interface IDataAccess
{
    Task<CartEntity> GetCart(Guid cartId);
    Task AddItemToCart(CartEntity cart, ItemEntity item);
}

public class DataAccess : IDataAccess
{
    public IConfiguration Configuration { get; set; }

    public Task AddItemToCart(CartEntity cart, ItemEntity item)
    {
        var connectionString = Configuration.GetConnectionString(InfrastructureData.connectionName);
        using var db = new LiteDatabase(connectionString);
        var col = db.GetCollection<CartEntity>(InfrastructureData.collectionName);

        cart.ItemsInCart.Add(item);

        col.Update(cart);

        return Task.CompletedTask;
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
