using iBurguer.ShoppingCart.Core.Abstractions;
using static iBurguer.ShoppingCart.Core.Domain.Exceptions; 

namespace iBurguer.ShoppingCart.Core.Domain;

public class CartItem
{
    public Guid CartItemId { get; set; }
    
    public Product Product { get; private set; }
    
    public Quantity Quantity { get; private set; }
    
    public Price UnitPrice => Product.Price;

    public Price Subtotal => Quantity * UnitPrice;
    
    public CartItem(Product product, Quantity quantity)
    {
        CartItemId = Guid.NewGuid();
        Product = product;
        Quantity = quantity;
    }
    
    public void UpdatePrice(Price newPrice) => Product.UpdatePrice(newPrice);
}