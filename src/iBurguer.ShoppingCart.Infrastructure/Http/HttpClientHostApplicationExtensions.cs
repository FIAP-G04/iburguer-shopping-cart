using System.Diagnostics.CodeAnalysis;
using iBurguer.ShoppingCart.Core.Gateways;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace iBurguer.ShoppingCart.Infrastructure.Http;

[ExcludeFromCodeCoverage]
public static class HttpClientHostApplicationExtensions
{
    public static IHostApplicationBuilder AddMenuRestClient(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetRequiredSection("MenuApi").Get<MenuApiConfiguration>();

        configuration!.ThrowIfInvalid();

        builder.Services.AddHttpClient<IMenuGateway, MenuApiClient>(options =>
        {
            options.BaseAddress = new Uri(configuration.Url);
            options.DefaultRequestHeaders.Clear();
        })
            .AddResilienceHandler("MenuApi", options =>
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
        
        builder.Services.AddSingleton(configuration);
        
        builder.Services.AddScoped<IMenuGateway, MenuApiClient>();

        return builder;
    }
}