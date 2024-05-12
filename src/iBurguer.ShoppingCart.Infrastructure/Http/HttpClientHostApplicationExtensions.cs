using System.Diagnostics.CodeAnalysis;
using iBurguer.ShoppingCart.Core.Gateways;
using iBurguer.ShoppingCart.Infrastructure.Http.Menu;
using iBurguer.ShoppingCart.Infrastructure.Http.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace iBurguer.ShoppingCart.Infrastructure.Http;

[ExcludeFromCodeCoverage]
public static class HttpClientHostApplicationExtensions
{
    public static IHostApplicationBuilder AddRestClient(this IHostApplicationBuilder builder)
    {
        var menuConfig = builder.Configuration.GetRequiredSection("Menu").Get<MenuApiConfiguration>();

        menuConfig!.ThrowIfInvalid();
        
        builder.Services.AddSingleton(menuConfig);
        
        var orderConfig = builder.Configuration.GetRequiredSection("Order").Get<OrderApiConfiguration>();
        
        orderConfig!.ThrowIfInvalid();
        
        builder.Services.AddSingleton(orderConfig);

        builder.AddHttpRestClient<IMenuGateway, MenuApiClient>(menuConfig);
        builder.AddHttpRestClient<IOrderGateway, OrderApiClient>(orderConfig);
        
        return builder;
    }

    private static IHostApplicationBuilder AddHttpRestClient<TClient, TImplementation>(this IHostApplicationBuilder builder, ApiConfiguration configuration) 
        where TClient : class
        where TImplementation : class, TClient
    {
        builder.Services.AddHttpClient<TClient, TImplementation>(options =>
        {
            options.BaseAddress = new Uri(configuration.Url);
            options.DefaultRequestHeaders.Clear();
        })
            .AddResilience(configuration);
        
        builder.Services.AddScoped<TClient, TImplementation>();

        return builder;
    }
    

    private static IHttpClientBuilder AddResilience(this IHttpClientBuilder builder, ApiConfiguration configuration)
    {
        builder
            .AddResilienceHandler(configuration.Tag(), options =>
            {
                options
                    .AddRetry(new HttpRetryStrategyOptions()
                    {
                        MaxRetryAttempts = configuration.MaxRetryAttempts, 
                        Delay = TimeSpan.FromMilliseconds(configuration.RetryDelay)
                    })
                    .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions()
                    {
                        MinimumThroughput = configuration.CircuitBreakerMinimumThroughput, 
                        BreakDuration = TimeSpan.FromSeconds(configuration.CircuitBreakerBreakDuration)
                    })
                    .AddTimeout(new HttpTimeoutStrategyOptions()
                    {
                        Timeout = TimeSpan.FromSeconds(configuration.Timeout)
                    });
            });
        
        return builder;
    }
}