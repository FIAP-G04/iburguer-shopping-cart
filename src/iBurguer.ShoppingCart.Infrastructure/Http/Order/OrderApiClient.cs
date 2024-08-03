using System.Text;
using System.Text.Json;
using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;
using iBurguer.ShoppingCart.Core.UseCases.Checkout;
using Microsoft.Extensions.Logging;

namespace iBurguer.ShoppingCart.Infrastructure.Http.Order;

public class OrderApiClient : IOrderGateway
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<OrderApiClient> _logger;

    public OrderApiClient(OrderApiConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<OrderApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(logger);
        
        _baseUrl = configuration.Url;
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }
    
    public async Task<CheckoutResponse> RegisterOrder(Cart cart, string orderType, CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.Serialize(RegisterOrderRequest.Convert(cart, orderType));
        
        var payload = new StringContent(request, Encoding.UTF8, "application/json");
        
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/api/orders", payload, cancellationToken);
            
            response.EnsureSuccessStatusCode();
            
            var value = await response.Content.ReadAsStringAsync();
            
            var result = JsonSerializer.Deserialize<CheckoutResponse>(value, new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true
            });

            return result!;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while trying to confirm an order.", ex);

            throw;
        }
    }
}