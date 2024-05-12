using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.GetCart;

public interface IGetCartUseCase
{
    Task<ShoppingCartResponse> GetCartById(Guid shoppingCartId, CancellationToken cancellation);
}
public class GetCartUseCase : IGetCartUseCase
{
    private readonly IShoppingCartRepository _repository;

    public GetCartUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }
    
    public async Task<ShoppingCartResponse> GetCartById(Guid shoppingCartId, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(shoppingCartId, cancellation);

        ShoppingCartNotFoundException.ThrowIfNull(shoppingCart);

        return ShoppingCartResponse.Convert(shoppingCart);
    }
}
