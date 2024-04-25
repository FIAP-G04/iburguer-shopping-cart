using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Domain.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.ClearCart;

public interface IClearShoppingCartUseCase
{
    Task ClearShoppingCart(Guid shoppingCartId, CancellationToken cancellation);
}

public class ClearShoppingCartUseCase: IClearShoppingCartUseCase
{
    private readonly IShoppingCartRepository _repository;

    public ClearShoppingCartUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task ClearShoppingCart(Guid shoppingCartId, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(shoppingCartId, cancellation);

        Exceptions.ShoppingCartNotFound.ThrowIfNull(shoppingCart);

        shoppingCart!.Clear();

        await _repository.Save(shoppingCart, cancellation);
    }
}
