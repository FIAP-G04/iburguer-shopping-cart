using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace iBurguer.ShoppingCart.Infrastructure.Redis;

[ExcludeFromCodeCoverage]
public static class RedisHostApplicationExtensions
{
    public static IHostApplicationBuilder AddRedis(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetRequiredSection("Redis").Get<RedisConfiguration>();

        configuration!.ThrowIfInvalid();

        builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            ConnectionMultiplexer.Connect(configuration.ConnectionString));
        
        builder.Services.AddSingleton(configuration);
        builder.Services.AddScoped<ICacheContext, CacheContext>();
            
        return builder;
    }
}