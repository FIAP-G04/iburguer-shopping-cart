using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.Checkout;

namespace iBurguer.ShoppingCart.Core.Gateways;

public interface IOrderGateway
{
    Task<CheckoutResponse> RegisterOrder(Cart cart, string orderType, CancellationToken cancellationToken);
}