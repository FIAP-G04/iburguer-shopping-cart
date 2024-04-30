using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Infrastructure.Redis.Models
{
    public class CartItemModel
    {
        public Guid CartItemId { get; set; }
        public ProductModel Product { get; set; }
        public ushort Quantity { get; set; }

        public static CartItemModel Map(CartItem item)
            => new()
            {
                CartItemId = item.CartItemId,
                Product = ProductModel.Map(item.Product),
                Quantity = item.Quantity.Value
            };
    }
}
