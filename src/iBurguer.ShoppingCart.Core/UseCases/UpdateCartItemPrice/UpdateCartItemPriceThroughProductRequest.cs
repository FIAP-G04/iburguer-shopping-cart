namespace iBurguer.ShoppingCart.Core.UseCases.UpdateCartItemPrice;

public record UpdateCartItemPriceThroughProductRequest(Guid ShoppingCartId, Guid ProductId, decimal Price);