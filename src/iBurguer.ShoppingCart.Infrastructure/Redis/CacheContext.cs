using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using iBurguer.ShoppingCart.Core.Abstractions;
using iBurguer.ShoppingCart.Core.Domain;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace iBurguer.ShoppingCart.Infrastructure.Redis;

public interface ICacheContext
{
    Task Set(string groupName, string key, object value, CancellationToken cancellationToken);

    Task<T?> Get<T>(string groupName, string key, CancellationToken cancellationToken) where T : class;
}

public class CacheContext : ICacheContext
{
    private readonly IConnectionMultiplexer _redis;
    private readonly TimeSpan _expire;

    public CacheContext(IConnectionMultiplexer redis, RedisConfiguration configuration)
    {
        _redis = redis;
        _expire = TimeSpan.FromMinutes(configuration.SlidingExpirationInMinutes);
    }
    
    public async Task Set(string groupName, string key, object value, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();

        var serialized = JsonSerializer.Serialize(value, new JsonSerializerOptions()
        {
            IgnoreReadOnlyProperties = false,
            Converters = { new ProductTypeJsonConverter() }
        });

        await db.StringSetAsync($"{groupName}.{key}", serialized, _expire);
    }

    public async Task<T?> Get<T>(string groupName, string key, CancellationToken cancellationToken) where T : class
    {
        var db = _redis.GetDatabase();
        
        var value = await db.StringGetAsync($"{groupName}.{key}");

        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions()
            {
                IncludeFields = true,
                IgnoreReadOnlyProperties = false,
                Converters = { new ProductTypeJsonConverter() }
                
            });
        }

        return null;
    }
}

public class ProductTypeJsonConverter : JsonConverter<ProductType>
{
    public override ProductType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();
        var name = reader.GetString();
        return ProductType.FromName(name);
    }

    public override void Write(Utf8JsonWriter writer, ProductType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}