

using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Core.Gateways;

public interface IMenuGateway
{
    Task<Product> GetProductDetailsFromTheMenu(Guid productId, CancellationToken cancellationToken = default);
}