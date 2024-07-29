using static iBurguer.ShoppingCart.Core.Exceptions;
using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;
using iBurguer.ShoppingCart.Core.Abstractions;

namespace iBurguer.ShoppingCart.Core.UseCases.Checkout;

public interface ICheckoutUseCase
{
    Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancellation);
}

public class CheckoutUseCase : ICheckoutUseCase
{
    private readonly IShoppingCartRepository _repository;
    private readonly IOrderGateway _gateway;
    private readonly ISQSService _sqsService;

    public CheckoutUseCase(IShoppingCartRepository repository, IOrderGateway gateway, ISQSService sqsService)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(gateway);

        _repository = repository;
        _gateway = gateway;
        _sqsService = sqsService;
    }

    public async Task<CheckoutResponse> Checkout(CheckoutRequest request, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(request.ShoppingCartId, cancellation);

        ShoppingCartNotFoundException.ThrowIfNull(shoppingCart);

        shoppingCart!.Close();

        var cartClosedEvent = CartClosedDomainEvent.Convert(shoppingCart, request.OrderType);
        await _sqsService.SendMessage(cartClosedEvent, cancellation);
        
        await _repository.Save(shoppingCart, cancellation);

        return null;
    }

}
