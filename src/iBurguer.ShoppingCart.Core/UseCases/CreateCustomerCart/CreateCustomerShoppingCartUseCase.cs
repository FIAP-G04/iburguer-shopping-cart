using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.CreateCustomerCart;

public interface ICreateCustomerShoppingCartUseCase
{
    Task<CreateShoppingCartResponse> CreateCustomerShoppingCart(CreateShoppingCartRequest request, CancellationToken cancellation);
}

public class CreateCustomerShoppingCartUseCase : ICreateCustomerShoppingCartUseCase
{
    private readonly IShoppingCartRepository _repository;

    public CreateCustomerShoppingCartUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<CreateShoppingCartResponse> CreateCustomerShoppingCart(CreateShoppingCartRequest request, CancellationToken cancellation)
    {
        var shoppingCart = Cart.CreateCustomerShoppingCart(request.CustomerId);

        await _repository.Save(shoppingCart, cancellation);

        return new CreateShoppingCartResponse(shoppingCart.Id, request.CustomerId);
    }
}
