using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Core.UseCases.UpdateCartItemPrice;

public interface IUpdateCartItemPriceThroughProductUseCase
{
    Task UpdateCartItemPriceThroughProduct(UpdateCartItemPriceThroughProductRequest request, CancellationToken cancellation);
}

public class UpdateCartItemPriceThroughProductUseCase : IUpdateCartItemPriceThroughProductUseCase
{
    private readonly IShoppingCartRepository _repository;

    public UpdateCartItemPriceThroughProductUseCase(IShoppingCartRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task UpdateCartItemPriceThroughProduct(UpdateCartItemPriceThroughProductRequest request, CancellationToken cancellation)
    {
        var shoppingCart = await _repository.GetById(request.ShoppingCartId, cancellation);

        Exceptions.ShoppingCartNotFound.ThrowIfNull(shoppingCart);

        shoppingCart!.UpdateItemPriceThroughProduct(request.ProductId, request.Price);

        await _repository.Save(shoppingCart, cancellation);
    }
}
