using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Domain.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.CloseCart;

public interface ICloseShoppingCartUseCase
{
    Task CloseShoppingCart(Guid shoppingCartId, CancellationToken cancellation);
}
    public class CloseShoppingCartUseCase : ICloseShoppingCartUseCase
{
    private readonly IShoppingCartRepository _repository;

    public CloseShoppingCartUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }
    public async Task CloseShoppingCart(Guid shoppingCartId, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(shoppingCartId, cancellation);

        Exceptions.ShoppingCartNotFound.ThrowIfNull(shoppingCart);

        shoppingCart!.Close();

        await _repository.Save(shoppingCart, cancellation);
    }
}
