using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.CreateAnonymousCart;

public interface ICreateAnonymousShoppingCartUseCase
{
    Task<CreateAnonymousShoppingCartResponse> CreateAnonymousShoppingCart(CancellationToken cancellation);
}

public class CreateAnonymousShoppingCartUseCase : ICreateAnonymousShoppingCartUseCase
{
    private readonly IShoppingCartRepository _repository;

    public CreateAnonymousShoppingCartUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<CreateAnonymousShoppingCartResponse> CreateAnonymousShoppingCart(CancellationToken cancellation)
    {
        var shoppingCart = Cart.CreateAnonymousShoppingCart();

        await _repository.Save(shoppingCart, cancellation);

        return new CreateAnonymousShoppingCartResponse(shoppingCart.Id);
    }
}
