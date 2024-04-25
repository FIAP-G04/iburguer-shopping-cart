using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Core.UseCases.GetCart;

public record ShoppingCartResponse
{
    public Guid ShoppingCartId { get; set; }
    public Guid? CustomerId { get; set; }
    public bool Closed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Decimal Total { get; set; }
    public IList<ShoppingCartItemResponse> Items { get; set; } = new List<ShoppingCartItemResponse>();

    public static ShoppingCartResponse Convert(Cart shoppingCart)
    {
        var response = new ShoppingCartResponse
        {
            ShoppingCartId = shoppingCart.Id,
            CustomerId = shoppingCart.CustomerId,
            CreatedAt = shoppingCart.CreatedAt,
            UpdatedAt = shoppingCart.UpdatedAt,
            Total = shoppingCart.Total
        };

        if (response.Items.Any())
        {
            response.Items = shoppingCart.Items.Select(item => ShoppingCartItemResponse.Convert(item)).ToList();
        }

        return response;
    }
}
