using System.Text.Json;
using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;

namespace iBurguer.ShoppingCart.Infrastructure.Http;

public class MenuApiClient : IMenuGateway
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public MenuApiClient(MenuApiConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        
        _baseUrl = configuration.Url;
        _httpClient = httpClientFactory.CreateClient();
    }
    
    public async Task<Product> GetProductDetailsFromTheMenu(Guid productId, CancellationToken cancellationToken = default)
    {
        string requestUrl = $"{_baseUrl}/api/menu/items/{productId}";
        
        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl, cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var value = await response.Content.ReadAsStringAsync();
        
        var menuItem = JsonSerializer.Deserialize<MenuItemResponse>(value, new JsonSerializerOptions 
        {
            PropertyNameCaseInsensitive = true
        });
        
        return menuItem!.Convert();
    }
}