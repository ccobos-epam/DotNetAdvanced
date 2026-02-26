namespace CartWebApi.Features.Cart.CreateCart.V01;

public interface IRepository
{
  Task CreateCart(Entities.Cart cart, CancellationToken ct =  default);
}