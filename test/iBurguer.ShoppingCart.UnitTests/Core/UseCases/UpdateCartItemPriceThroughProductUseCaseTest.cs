using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.UpdateCartItemPrice;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class UpdateCartItemPriceThroughProductUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly UpdateCartItemPriceThroughProductUseCase _useCase;

        public UpdateCartItemPriceThroughProductUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new UpdateCartItemPriceThroughProductUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task UpdateCartItemPriceThroughProduct_ShouldUpdateItemPriceAndSaveCart()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var shoppingCartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newPrice = new Price(15);
            var request = new UpdateCartItemPriceThroughProductRequest(shoppingCartId, productId, newPrice);
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product(productId, "Test Product", ProductType.MainDish, new Price(10));
            cart.AddCartItem(product, new Quantity(5));

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.UpdateCartItemPriceThroughProduct(request, cancellation);

            // Assert
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCartItemPriceThroughProduct_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var shoppingCartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newPrice = new Price(15);
            var request = new UpdateCartItemPriceThroughProductRequest(shoppingCartId, productId, newPrice);

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.UpdateCartItemPriceThroughProduct(request, cancellation));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UpdateCartItemPriceThroughProductUseCase(null));
        }
    }
}
