using System.Diagnostics.CodeAnalysis;
using iBurguer.ShoppingCart.Core.UseCases.AddItem;
using iBurguer.ShoppingCart.Core.UseCases.Checkout;
using iBurguer.ShoppingCart.Core.UseCases.ClearCart;
using iBurguer.ShoppingCart.Core.UseCases.CloseCart;
using iBurguer.ShoppingCart.Core.UseCases.CreateAnonymousCart;
using iBurguer.ShoppingCart.Core.UseCases.CreateCustomerCart;
using iBurguer.ShoppingCart.Core.UseCases.DecrementCartItem;
using iBurguer.ShoppingCart.Core.UseCases.GetCart;
using iBurguer.ShoppingCart.Core.UseCases.IncrementCartItem;
using iBurguer.ShoppingCart.Core.UseCases.RemoveCartItem;
using iBurguer.ShoppingCart.Core.UseCases.UpdateCartItemPrice;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.ShoppingCart.Infrastructure.IoC;

[ExcludeFromCodeCoverage]
public static class UseCaseHostApplicationExtensions
{
    public static IHostApplicationBuilder AddUseCases(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IAddItemToShoppingCartUseCase, AddItemToShoppingCartUseCase>()
            .AddScoped<IClearShoppingCartUseCase, ClearShoppingCartUseCase>()
            .AddScoped<ICloseShoppingCartUseCase, CloseShoppingCartUseCase>()
            .AddScoped<ICreateAnonymousShoppingCartUseCase, CreateAnonymousShoppingCartUseCase>()
            .AddScoped<ICreateCustomerShoppingCartUseCase, CreateCustomerShoppingCartUseCase>()
            .AddScoped<IDecrementTheQuantityOfTheCartItemUseCase, DecrementTheQuantityOfTheCartItemUseCase>()
            .AddScoped<IIncrementTheQuantityOfTheCartItemUseCase, IncrementTheQuantityOfTheCartItemUseCase>()
            .AddScoped<IRemoveCartItemFromShoppingCartUseCase, RemoveCartItemFromShoppingCartUseCase>()
            .AddScoped<IUpdateCartItemPriceThroughProductUseCase, UpdateCartItemPriceThroughProductUseCase>()
            .AddScoped<IGetCartUseCase, GetCartUseCase>()
            .AddScoped<ICheckoutUseCase, CheckoutUseCase>();

        return builder;
    }
}