namespace CartService.UseCases.GetList;

public interface IBusinessLogic
{
    Task<ResponseDto> GetListOfItemsinCart(Guid idOfCart);
}

public class BusinessLogic : IBusinessLogic
{
    public IDataAccess DataAccess { get; set; }
    public async Task<ResponseDto> GetListOfItemsinCart(Guid idOfCart)
    {
        var daoResult = await DataAccess.GetListOfItemsinCart(idOfCart);
        var result = daoResult.Select(i => new ItemDto
        {
            Id = i.Id,
            Name = i.Name,
            ImageURL = i.Image?.URLOfImage,
            ImageAltText = i.Image?.AltText,
            Price = i.Price,
            Quantity = i.Quantity
        })
        .ToList();
        var objectToReturn = new ResponseDto { Results = result };
        return objectToReturn;
    }
}
