using CartService.DataStorage;
using CartService.Entities;
using LiteDB;

namespace CartService.UseCases.CreateCart;


public interface IDataAccess
{
    Task<CartEntity> CreateNewCart(Guid id);
}

public class DataAccess : IDataAccess
{
    public IConfiguration Configuration { get; set; }

    public Task<CartEntity> CreateNewCart(Guid id)
    {
        var connectionString = Configuration.GetConnectionString(InfrastructureData.connectionName);
        using var db = new LiteDatabase(connectionString);
        var col = db.GetCollection<CartEntity>(InfrastructureData.collectionName);

        var newCart = new CartEntity { Id = id, ItemsInCart = [] };

        col.Insert(newCart);

        return Task.FromResult(newCart);
    }
}
