using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.Checkout;
using iBurguer.ShoppingCart.Infrastructure.Http.Menu;
using iBurguer.ShoppingCart.Infrastructure.Http.Order;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace iBurguer.ShoppingCart.UnitTests.Infrastructure.Http
{
    public class OrderApiClientTest
    {
        private readonly OrderApiConfiguration _configuration;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<OrderApiClient>> _loggerMock;
        private readonly OrderApiClient _orderApiClient;

        public OrderApiClientTest()
        {
            _configuration = new OrderApiConfiguration { Url = "https://example.com" };

            _handlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(_configuration.Url)
            };

            _loggerMock = new Mock<ILogger<OrderApiClient>>();

            _orderApiClient = new OrderApiClient(_configuration, new HttpClientFactoryMock(_httpClient), _loggerMock.Object);
        }

        [Fact]
        public async Task RegisterOrder_ValidRequest_ReturnsCheckoutResponse()
        {
            // Arrange
            var cart = new Cart(); // Populate with required values
            var orderType = "TestOrderType";
            var expectedResponse = new CheckoutResponse { /* Populate with expected values */ };
            var expectedContent = JsonConvert.SerializeObject(expectedResponse);
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
            var result = await _orderApiClient.RegisterOrder(cart, orderType);

            // Assert
            Assert.NotNull(result);
            // Add additional assertions based on the expected values
        }

        [Fact]
        public async Task RegisterOrder_HttpRequestException_ThrowsExceptionAndLogsError()
        {
            // Arrange
            var cart = new Cart(); // Populate with required values
            var orderType = "TestOrderType";
            _handlerMock.Protected()
                        .Setup<Task<HttpResponseMessage>>(
                            "SendAsync",
                            ItExpr.IsAny<HttpRequestMessage>(),
                            ItExpr.IsAny<CancellationToken>())
                        .ThrowsAsync(new HttpRequestException("Request failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _orderApiClient.RegisterOrder(cart, orderType));
            Assert.Equal("Request failed", exception.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while trying to confirm an order.")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
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
