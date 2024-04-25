using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Core.UseCases.GetCart;

public record ShoppingCartItemResponse
{
    public Guid CartItemId { get; set; }
    
    public Guid ProductId { get; set; }
    
    public string ProductName { get; set; }
    
    public string ProductType { get; set; }
    
    public Decimal UnitPrice { get; set; }
    
    public ushort Quantity { get; set; }

    public Decimal Subtotal { get; set; }

    public static ShoppingCartItemResponse Convert(CartItem shoppingCartItem)
    {
        return new ShoppingCartItemResponse
        {
            CartItemId = shoppingCartItem.CartItemId,
            ProductId = shoppingCartItem.Product.ProductId,
            ProductName = shoppingCartItem.Product.Name,
            ProductType = shoppingCartItem.Product.Type.ToString(),
            UnitPrice = shoppingCartItem.UnitPrice,
            Quantity = shoppingCartItem.Quantity,
            Subtotal = shoppingCartItem.Subtotal
        };
    }
    
}