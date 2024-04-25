using System.Diagnostics.CodeAnalysis;
using iBurguer.ShoppingCart.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.ShoppingCart.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public static class RepositoryHostApplicationExtensions
{
    public static IHostApplicationBuilder AddRepositories(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        return builder;
    }
}