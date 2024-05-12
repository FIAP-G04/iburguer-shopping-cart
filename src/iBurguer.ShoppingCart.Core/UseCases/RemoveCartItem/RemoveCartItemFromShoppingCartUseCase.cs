using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.RemoveCartItem;

public interface  IRemoveCartItemFromShoppingCartUseCase
{
    Task RemoveCartItemFromShoppingCart(Guid shoppingCartId, Guid cartItemId, CancellationToken cancellation);
}

public class RemoveCartItemFromShoppingCartUseCase : IRemoveCartItemFromShoppingCartUseCase
{
    private readonly IShoppingCartRepository _repository;

    public RemoveCartItemFromShoppingCartUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task RemoveCartItemFromShoppingCart(Guid shoppingCartId, Guid cartItemId, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(shoppingCartId, cancellation);

        Exceptions.ShoppingCartNotFoundException.ThrowIfNull(shoppingCart);

        shoppingCart!.RemoveCartItem(cartItemId);

        await _repository.Save(shoppingCart, cancellation);
    }
}
