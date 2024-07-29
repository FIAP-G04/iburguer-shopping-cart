using iBurguer.ShoppingCart.Core.Abstractions;
using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;
using iBurguer.ShoppingCart.Core.UseCases.Checkout;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class CheckoutUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly Mock<IOrderGateway> _gatewayMock;
        private readonly Mock<ISQSService> _sqsService;
        private readonly CheckoutUseCase _useCase;

        public CheckoutUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _gatewayMock = new Mock<IOrderGateway>();
            _sqsService = new Mock<ISQSService>();
            _useCase = new CheckoutUseCase(_repositoryMock.Object, _gatewayMock.Object, _sqsService.Object);
        }

        [Fact]
        public async Task Checkout_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var request = new CheckoutRequest
            {
                ShoppingCartId = Guid.NewGuid(),
                OrderType = "Online"
            };

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.Checkout(request, CancellationToken.None));
        }

        [Fact]
        public async Task Checkout_ShouldCloseCartAndRegisterOrder()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            cart.AddCartItem(new Product(Guid.NewGuid(), "tst", ProductType.MainDish, new Price(10)), new Quantity(1));
            var request = new CheckoutRequest
            {
                ShoppingCartId = cart.Id,
                OrderType = "Online"
            };
            var expectedResponse = new CheckoutResponse
            {
                OrderId = Guid.NewGuid()
            };

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _gatewayMock.Setup(gateway => gateway.RegisterOrder(It.IsAny<Cart>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _useCase.Checkout(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedResponse.OrderId, response.OrderId);
            Assert.True(cart.Closed);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
            _gatewayMock.Verify(gateway => gateway.RegisterOrder(It.IsAny<Cart>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CheckoutUseCase(null, _gatewayMock.Object, _sqsService.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenGatewayIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CheckoutUseCase(_repositoryMock.Object, null, _sqsService.Object));
        }
    }
}
