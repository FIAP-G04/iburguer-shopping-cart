using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Core.UseCases.AddItem;

public record AddItemToShoppingCartResponse
{
    public required Guid ShoppingCartId { get; set; }
    public required Guid ItemCartId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? UpdatedAt { get; set; }

    public static AddItemToShoppingCartResponse Convert(Cart shoppingCart, Guid CartItemId)
    {
        return new AddItemToShoppingCartResponse
        {
            ShoppingCartId = shoppingCart.Id,
            ItemCartId = CartItemId,
            CreatedAt = shoppingCart.CreatedAt,
            UpdatedAt =  shoppingCart.UpdatedAt
        };
    }
}