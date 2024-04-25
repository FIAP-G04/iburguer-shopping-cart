namespace iBurguer.ShoppingCart.Core.UseCases.DecrementCartItem;

public record DecrementTheQuantityOfTheCartItemRequest(Guid ShoppingCartId, Guid CartItemId, ushort quantity);