

using OneOf;
using OneOf.Types;

namespace CartWebApi.Features.Cart.CreateCart.V01;

public class Handler
{
  public virtual async Task<OneOf<Success, Error>> CreateCart(Guid cartId)
  {
    return await Task.FromResult<OneOf<Success, Error>>(cartId != Guid.Empty ? new Success() : new Error());
  }
}