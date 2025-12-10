using CartService.Entities;

namespace CartService.UseCases.GetCart.V01;

public interface IRepository
{
    Task<CartEntity?> GetById(Guid id);
}
