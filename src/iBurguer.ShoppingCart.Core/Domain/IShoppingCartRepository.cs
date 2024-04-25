namespace iBurguer.ShoppingCart.Core.Domain;

public interface IShoppingCartRepository
{
    Task Save(Cart shoppingCart, CancellationToken cancellationToken);

    Task<Cart?> GetById(Guid shoppingCartId, CancellationToken cancellationToken);
}