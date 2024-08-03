using iBurguer.ShoppingCart.Core.Abstractions;

namespace iBurguer.ShoppingCart.Core.Domain;

public record CartClosedDomainEvent : IDomainEvent
{
    public string OrderType { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = "QRCode";
    public Guid? BuyerId { get; init; }
    public IList<OrderItemRequest> Items { get; init; } = new List<OrderItemRequest>();

    public static CartClosedDomainEvent FromCart(Cart shoppingCart, string orderType)
    {
        return new CartClosedDomainEvent
        {
            OrderType = orderType,
            BuyerId = shoppingCart.CustomerId,
            Items = shoppingCart.Items.Select(OrderItemRequest.FromCartItem).ToList()
        };
    }
}

public record OrderItemRequest
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string ProductType { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public ushort Quantity { get; init; }

    public static OrderItemRequest FromCartItem(CartItem item)
    {
        return new OrderItemRequest
        {
            ProductId = item.Product.ProductId,
            ProductName = item.Product.Name,
            ProductType = item.Product.Type.Name,
            UnitPrice = item.UnitPrice,
            Quantity = item.Quantity
        };
    }
}