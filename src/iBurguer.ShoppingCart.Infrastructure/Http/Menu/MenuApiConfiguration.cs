using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.Http.Menu;

[ExcludeFromCodeCoverage]
public class MenuApiConfiguration : ApiConfiguration
{
    public override string Tag() => "Menu";
}