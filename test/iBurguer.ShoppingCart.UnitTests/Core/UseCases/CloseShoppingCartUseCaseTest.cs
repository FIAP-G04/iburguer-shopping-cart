using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.CloseCart;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class CloseShoppingCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly CloseShoppingCartUseCase _useCase;

        public CloseShoppingCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new CloseShoppingCartUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task CloseShoppingCart_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.CloseShoppingCart(shoppingCartId, CancellationToken.None));
        }

        [Fact]
        public async Task CloseShoppingCart_ShouldCloseCartAndSave()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            cart.AddCartItem(new Product(Guid.NewGuid(), "tst", ProductType.MainDish, new Price(10)), new Quantity(1));
            var shoppingCartId = cart.Id;

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.CloseShoppingCart(shoppingCartId, CancellationToken.None);

            // Assert
            Assert.True(cart.Closed);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CloseShoppingCartUseCase(null));
        }
    }
}
