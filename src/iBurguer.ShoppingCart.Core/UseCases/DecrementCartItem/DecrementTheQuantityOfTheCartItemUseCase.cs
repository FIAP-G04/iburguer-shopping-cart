using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.DecrementCartItem;

public interface IDecrementTheQuantityOfTheCartItemUseCase
{
    Task DecrementTheQuantityOfTheCartItem(DecrementTheQuantityOfTheCartItemRequest request, CancellationToken cancellation);
}

public class DecrementTheQuantityOfTheCartItemUseCase : IDecrementTheQuantityOfTheCartItemUseCase
{
    private readonly IShoppingCartRepository _repository;

    public DecrementTheQuantityOfTheCartItemUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }
    public async Task DecrementTheQuantityOfTheCartItem(DecrementTheQuantityOfTheCartItemRequest request, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(request.ShoppingCartId, cancellation);

        Exceptions.ShoppingCartNotFoundException.ThrowIfNull(shoppingCart);

        shoppingCart!.DecrementTheQuantityOfTheCartItem(request.CartItemId, request.quantity);

        await _repository.Save(shoppingCart, cancellation);
    }
}
