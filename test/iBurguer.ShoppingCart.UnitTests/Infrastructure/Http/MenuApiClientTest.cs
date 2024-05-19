using iBurguer.ShoppingCart.Infrastructure.Http.Menu;
using Moq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Moq.Protected;

namespace iBurguer.ShoppingCart.UnitTests.Infrastructure.Http
{
    public class MenuApiClientTest
    {
        private readonly MenuApiConfiguration _configuration;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly MenuApiClient _menuApiClient;

        public MenuApiClientTest()
        {
            _configuration = new MenuApiConfiguration { Url = "https://example.com" };

            _handlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(_configuration.Url)
            };

            _menuApiClient = new MenuApiClient(_configuration, new HttpClientFactoryMock(_httpClient));
        }

        [Fact]
        public async Task GetProductDetailsFromTheMenu_ValidProductId_ReturnsProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var menuResponse = new MenuItemResponse
            {
                Id = Guid.NewGuid(),
                Category = "MainDish",
                Description = "TST",
                ImagesUrl = new string[] { },
                Name = "TST",
                PreparationTime = 10,
                Price = 10
            };
            var expectedContent = JsonConvert.SerializeObject(menuResponse);

            _handlerMock.Protected()
                        .Setup<Task<HttpResponseMessage>>(
                            "SendAsync",
                            ItExpr.IsAny<HttpRequestMessage>(),
                            ItExpr.IsAny<CancellationToken>())
                        .ReturnsAsync(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(expectedContent, Encoding.UTF8, "application/json")
                        });

            // Act
            var result = await _menuApiClient.GetProductDetailsFromTheMenu(productId);

            // Assert
            Assert.NotNull(result);
            // Add additional assertions based on the expected values
        }

        [Fact]
        public async Task GetProductDetailsFromTheMenu_ProductNotFound_ThrowsException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _handlerMock.Protected()
                        .Setup<Task<HttpResponseMessage>>(
                            "SendAsync",
                            ItExpr.IsAny<HttpRequestMessage>(),
                            ItExpr.IsAny<CancellationToken>())
                        .ReturnsAsync(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound
                        });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _menuApiClient.GetProductDetailsFromTheMenu(productId));
        }

        private class HttpClientFactoryMock : IHttpClientFactory
        {
            private readonly HttpClient _client;

            public HttpClientFactoryMock(HttpClient client)
            {
                _client = client;
            }

            public HttpClient CreateClient(string name = "")
            {
                return _client;
            }
        }
    }
}
