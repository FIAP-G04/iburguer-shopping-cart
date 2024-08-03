using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.Http.Order;

[ExcludeFromCodeCoverage]
public class OrderApiConfiguration : ApiConfiguration
{
    public override string Tag() => "Menu";
}