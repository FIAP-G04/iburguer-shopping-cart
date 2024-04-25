using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Distributed;

namespace iBurguer.ShoppingCart.Infrastructure.Redis;

[ExcludeFromCodeCoverage]
public record RedisConfiguration
{
    /// <summary>
    /// Connection string to the Redis cluster
    /// </summary>
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// The time in minutes that an object is removed from the cache regardless of when the object was accessed.
    /// </summary>
    public int AbsoluteExpirationInMinutes { get; set; } = 30;
    
    /// <summary>
    /// The time in minutes that an object will be removed from the cache if it hasn't been accessed.
    /// </summary>
    public int SlidingExpirationInMinutes { get; set; } = 5;

    /// <summary>
    /// Validates the Redis configuration.
    /// </summary>
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Redis ConnectionString is misconfigured.");
        }
        
        if (AbsoluteExpirationInMinutes <= 0)
        {
            throw new InvalidOperationException("Absolute expiration time must be greater than zero.");
        }
        
        if (SlidingExpirationInMinutes <= 0)
        {
            throw new InvalidOperationException("Sliding expiration time must be greater than zero.");
        }
        
        if (SlidingExpirationInMinutes >= AbsoluteExpirationInMinutes)
        {
            throw new InvalidOperationException("Sliding expiration time must be less than absolute expiration time.");
        }
    }

    public DistributedCacheEntryOptions TTLOptions => new()
    {
        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(AbsoluteExpirationInMinutes),
        SlidingExpiration = TimeSpan.FromMinutes(SlidingExpirationInMinutes)
    };
}
