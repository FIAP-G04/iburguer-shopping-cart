namespace iBurguer.ShoppingCart.Core.UseCases.Checkout;

public record CheckoutResponse
{
    public Guid OrderId { get; set; }
    public int OrderNumber { get; set; }
    public string PickupCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}