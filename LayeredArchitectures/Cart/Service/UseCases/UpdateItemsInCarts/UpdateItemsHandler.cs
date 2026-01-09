using CartService.DataStorage;
using CartService.Entities;
using Wolverine.Attributes;
using CommandContracts.RabbitMQ.Product.Update.V01;

namespace CartService.UseCases.UpdateItemsInCarts;

public class UpdateItemsHandler(LiteDbContext dbContext)
{
    //Dependencies
    private readonly LiteDbContext _dbContext = dbContext;

    [WolverineHandler]
    public void UpdateItemsWithNewPrice(UpdateCommand_V01 command)
    {
        var liteDbCollection = _dbContext.GetCollection<CartEntity>(InfrastructureData.collectionName);

        var tartgetsToupdate = liteDbCollection.Query()
            .Where(cart => cart.ItemsInCart.Any(item => item.Name == command.ProductName))
            .ToList();

        foreach (var tartget in tartgetsToupdate)
        {
            var objectToUpdate = tartget.ItemsInCart.FirstOrDefault(item => item.Name == command.ProductName);
            objectToUpdate?.Price = command.ProductPrice;
        }
        
    }
}