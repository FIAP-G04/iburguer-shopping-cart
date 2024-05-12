using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.Http.Menu;

[ExcludeFromCodeCoverage]
public class OrderApiConfiguration : ApiConfiguration
{
    public override string Tag() => "Menu";
}