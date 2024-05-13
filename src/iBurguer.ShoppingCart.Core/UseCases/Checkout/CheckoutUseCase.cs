using static iBurguer.ShoppingCart.Core.Exceptions;
using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;

namespace iBurguer.ShoppingCart.Core.UseCases.Checkout;

public interface ICheckoutUseCase
{
    Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancellation);
}

public class CheckoutUseCase : ICheckoutUseCase
{
    private readonly IShoppingCartRepository _repository;
    private readonly IOrderGateway _gateway;

    public CheckoutUseCase(IShoppingCartRepository repository, IOrderGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(gateway);

        _repository = repository;
        _gateway = gateway;
    }
    
    public async Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(request.ShoppingCartId, cancellation);

        ShoppingCartNotFoundException.ThrowIfNull(shoppingCart);

        shoppingCart!.Close();

        var order = await _gateway.RegisterOrder(shoppingCart, request.OrderType, cancellation);
        
        await _repository.Save(shoppingCart, cancellation);

        return order;
    }

}
