using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.IncrementCartItem;

public interface IIncrementTheQuantityOfTheCartItemUseCase
{
    Task IncrementTheQuantityOfTheCartItem(IncrementTheQuantityOfTheCartItemRequest request, CancellationToken cancellation);
}

public class IncrementTheQuantityOfTheCartItemUseCase : IIncrementTheQuantityOfTheCartItemUseCase
{
    private readonly IShoppingCartRepository _repository;

    public IncrementTheQuantityOfTheCartItemUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task IncrementTheQuantityOfTheCartItem(IncrementTheQuantityOfTheCartItemRequest request, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(request.ShoppingCartId, cancellation);

        ShoppingCartNotFoundException.ThrowIfNull(shoppingCart);

        shoppingCart.IncrementTheQuantityOfTheCartItem(request.CartItemId, request.quantity);

        await _repository.Save(shoppingCart, cancellation);
    }
}
