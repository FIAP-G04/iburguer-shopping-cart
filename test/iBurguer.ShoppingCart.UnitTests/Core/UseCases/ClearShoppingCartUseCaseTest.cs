using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.ClearCart;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class ClearShoppingCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly ClearShoppingCartUseCase _useCase;

        public ClearShoppingCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new ClearShoppingCartUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task ClearShoppingCart_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.ClearShoppingCart(shoppingCartId, CancellationToken.None));
        }

        [Fact]
        public async Task ClearShoppingCart_ShouldClearCartAndSave()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var shoppingCartId = cart.Id;

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.ClearShoppingCart(shoppingCartId, CancellationToken.None);

            // Assert
            Assert.Empty(cart.Items);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ClearShoppingCartUseCase(null));
        }
    }
}
