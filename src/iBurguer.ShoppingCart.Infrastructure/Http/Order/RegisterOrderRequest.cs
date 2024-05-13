using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Infrastructure.Http.Order;

public record RegisterOrderRequest
{
    public string OrderType { get; set; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;
    public Guid? BuyerId { get; init; }
    public IList<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    
    public static RegisterOrderRequest Convert(Cart shoppingCart, string orderType)
    {
        return new RegisterOrderRequest
        {
            OrderType = orderType,
            PaymentMethod = "QRCode",
            BuyerId = shoppingCart.CustomerId,
            Items =  shoppingCart.Items.Select(i => OrderItemRequest.Convert(i)).ToList()
        };
    }
}

public record OrderItemRequest
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public ushort Quantity { get; set; }
    
    public static OrderItemRequest Convert(CartItem item)
    {
        return new OrderItemRequest
        {
            ProductId = item.Product.ProductId,
            ProductName = item.Product.Name,
            ProductType = item.Product.Type.Name,
            UnitPrice =  item.UnitPrice,
            Quantity = item.Quantity
        };
    }
}