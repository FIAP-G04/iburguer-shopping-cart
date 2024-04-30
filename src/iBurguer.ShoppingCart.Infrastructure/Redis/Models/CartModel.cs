using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Infrastructure.Redis.Models
{
    public class CartModel
    {
        public IList<CartItemModel> Items { get; set; }
        public Guid? CustomerId { get; set; }
        public bool Closed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static CartModel Map(Cart cart)
            => new()
            {
                Items = cart.Items.Select(x => CartItemModel.Map(x)).ToList(),
                CustomerId = cart.CustomerId,
                Closed = cart.Closed,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
            };
    }
}
