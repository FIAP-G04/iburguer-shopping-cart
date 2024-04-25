using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;
using static iBurguer.ShoppingCart.Core.Domain.Exceptions;

namespace iBurguer.ShoppingCart.Core.UseCases.AddItem;

public interface IAddItemToShoppingCartUseCase
{
    Task<AddItemToShoppingCartResponse> AddItemToShoppingCart(AddItemToShoppingCartRequest request, CancellationToken cancellation);
}

public class AddItemToShoppingCartUseCase : IAddItemToShoppingCartUseCase
{
    private readonly IShoppingCartRepository _repository;
    private readonly IMenuGateway _gateway;

    public AddItemToShoppingCartUseCase(IShoppingCartRepository repository, IMenuGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(gateway);

        _repository = repository;
        _gateway = gateway;
    }
    
    public async Task<AddItemToShoppingCartResponse> AddItemToShoppingCart(AddItemToShoppingCartRequest request, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(request.ShoppingCartId, cancellation);

        ShoppingCartNotFound.ThrowIfNull(shoppingCart);

        var product = await _gateway.GetProductDetailsFromTheMenu(request.ProductId, cancellation);

        ProductNotFound.ThrowIfNull(product);

        var cartItem = shoppingCart!.AddCartItem(product, request.Quantity);

        await _repository.Save(shoppingCart, cancellation);
        
        return AddItemToShoppingCartResponse.Convert(shoppingCart, cartItem.CartItemId);
    }

}
