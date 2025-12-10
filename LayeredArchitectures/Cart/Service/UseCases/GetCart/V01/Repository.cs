using CartService.DataStorage;
using CartService.Entities;

namespace CartService.UseCases.GetCart.V01;

public class Repository : IRepository
{
    #region Dependency Injection Properties
    public LiteDbContext dbContext { get; set; }
    #endregion

    public async Task<CartEntity?> GetById(Guid id)
    {
        var liteDbCollection = dbContext.GetCollection<CartEntity>(InfrastructureData.collectionName);

        var possibleResult = liteDbCollection.Query()
            .Where(x => x.Id == id)
            .FirstOrDefault();

        return await Task.FromResult(possibleResult);
    }
}
