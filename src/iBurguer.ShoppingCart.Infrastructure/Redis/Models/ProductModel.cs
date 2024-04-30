using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Infrastructure.Redis.Models
{
    public class ProductModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }

        public static ProductModel Map(Product product)
            => new()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Type = product.Type.ToString(),
                Price = product.Price.Amount
            };
    }
}
