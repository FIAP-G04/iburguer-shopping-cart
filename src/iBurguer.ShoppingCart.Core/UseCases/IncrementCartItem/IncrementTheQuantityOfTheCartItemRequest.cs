namespace iBurguer.ShoppingCart.Core.UseCases.IncrementCartItem;

public record IncrementTheQuantityOfTheCartItemRequest(Guid ShoppingCartId, Guid CartItemId, ushort quantity);