using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.Gateways;
using iBurguer.ShoppingCart.Core.UseCases.AddItem;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class AddItemToShoppingCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly Mock<IMenuGateway> _gatewayMock;
        private readonly AddItemToShoppingCartUseCase _useCase;

        public AddItemToShoppingCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _gatewayMock = new Mock<IMenuGateway>();
            _useCase = new AddItemToShoppingCartUseCase(_repositoryMock.Object, _gatewayMock.Object);
        }

        [Fact]
        public async Task AddItemToShoppingCart_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var request = new AddItemToShoppingCartRequest
            {
                ShoppingCartId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = new Quantity(1)
            };

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.AddItemToShoppingCart(request, CancellationToken.None));
        }

        [Fact]
        public async Task AddItemToShoppingCart_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var request = new AddItemToShoppingCartRequest
            {
                ShoppingCartId = cart.Id,
                ProductId = Guid.NewGuid(),
                Quantity = new Quantity(1)
            };

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _gatewayMock.Setup(gateway => gateway.GetProductDetailsFromTheMenu(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() =>
                _useCase.AddItemToShoppingCart(request, CancellationToken.None));
        }

        [Fact]
        public async Task AddItemToShoppingCart_ShouldAddItemToCartAndSave()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product(Guid.NewGuid(), "Test Product", ProductType.MainDish, new Price(10.0m));
            var request = new AddItemToShoppingCartRequest
            {
                ShoppingCartId = cart.Id,
                ProductId = product.ProductId,
                Quantity = new Quantity(1)
            };

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _gatewayMock.Setup(gateway => gateway.GetProductDetailsFromTheMenu(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _useCase.AddItemToShoppingCart(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(cart.Id, response.ShoppingCartId);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AddItemToShoppingCartUseCase(null, _gatewayMock.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenGatewayIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AddItemToShoppingCartUseCase(_repositoryMock.Object, null));
        }
    }
}
