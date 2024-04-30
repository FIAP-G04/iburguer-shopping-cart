using iBurguer.ShoppingCart.Core.Domain;
using Newtonsoft.Json;
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

        var serialized = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            Converters = {
                    new ProductTypeJsonConverter(),
                    new PriceJsonConverter(),
                    new QuantityJsonConverter() 
            }
        });

        await db.StringSetAsync($"{groupName}.{key}", serialized, _expire);
    }

    public async Task<T?> Get<T>(string groupName, string key, CancellationToken cancellationToken) where T : class
    {
        var db = _redis.GetDatabase();
        
        var value = await db.StringGetAsync($"{groupName}.{key}");

        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings()
            {
                Converters = { 
                    new ProductTypeJsonConverter(),
                    new PriceJsonConverter(),
                    new QuantityJsonConverter()
                }
                
            });
        }

        return null;
    }
}

public class ProductTypeJsonConverter : JsonConverter<ProductType>
{
    public override ProductType? ReadJson(JsonReader reader, Type objectType, ProductType? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var name = reader.Value.ToString();
        return ProductType.FromName(name);
    }

    public override void WriteJson(JsonWriter writer, ProductType? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}

public class PriceJsonConverter : JsonConverter<Price>
{
    public override Price? ReadJson(JsonReader reader, Type objectType, Price? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var amount = reader.Value.ToString();
        decimal.TryParse(amount, out var price);
        return new Price(price);
    }

    public override void WriteJson(JsonWriter writer, Price? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Amount);
    }
}

public class QuantityJsonConverter : JsonConverter<Quantity>
{
    public override Quantity? ReadJson(JsonReader reader, Type objectType, Quantity? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = reader.Value.ToString();
        ushort.TryParse(value, out var quantity);
        return new Quantity(quantity);
    }

    public override void WriteJson(JsonWriter writer, Quantity? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Value);
    }
}