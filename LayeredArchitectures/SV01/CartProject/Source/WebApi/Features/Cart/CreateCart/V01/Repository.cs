using Marten;

namespace CartWebApi.Features.Cart.CreateCart.V01;

public class Repository(IDocumentSession session) : IRepository
{
  public Task CreateCart(Entities.Cart cart, CancellationToken ct = default)
  {
    session.Store(cart);
    return Task.CompletedTask;
  }
}