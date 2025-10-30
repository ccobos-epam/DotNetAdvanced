using CartService.DataStorage;
using CartService.Entities;
using LiteDB;

namespace CartService.UseCases.GetList;

public interface IDataAccess
{
    Task<List<ItemEntity>> GetListOfItemsinCart(Guid idOfCart);
}

public class DataAccess : IDataAccess
{
    public IConfiguration Configuration { get; set; }

    public Task<List<ItemEntity>> GetListOfItemsinCart(Guid idOfCart)
    {
        var connectionString = Configuration.GetConnectionString(InfrastructureData.connectionName);
        using var db = new LiteDatabase(connectionString);
        var col = db.GetCollection<CartEntity>(InfrastructureData.collectionName);

        var items = col.Query()
            .Where(cart => cart.Id == idOfCart)
            .Select(cart => cart.ItemsInCart)
            .FirstOrDefault();

        return Task.FromResult(items);
    }
}
