using Newtonsoft.Json;

namespace iBurguer.ShoppingCart.Core.Domain;

public class Product
{
    public Guid ProductId { get; set; }
    [JsonProperty]
    public string Name { get; private set; }
    [JsonProperty]
    public ProductType Type { get; private set; }
    public Price Price { get; private set; }

    public Product(Guid productId, string productName, ProductType productType, Price price)
    {
        ProductId = productId;
        Name = productName;
        Type = productType;
        Price = price;
    }
    
    public void UpdatePrice(Price newPrice) => Price = newPrice;
}