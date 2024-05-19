using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.RemoveCartItem;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class RemoveCartItemFromShoppingCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly RemoveCartItemFromShoppingCartUseCase _useCase;

        public RemoveCartItemFromShoppingCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new RemoveCartItemFromShoppingCartUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task RemoveCartItemFromShoppingCart_ShouldRemoveItemAndSaveCart()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var cart = Cart.CreateAnonymousShoppingCart();
            cart.AddCartItem(new Product(Guid.NewGuid(), "Test Product", ProductType.MainDish, new Price(10)), new Quantity(5));
            var shoppingCartId = cart.Id;
            var cartItemId = cart.Items.First().CartItemId;

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.RemoveCartItemFromShoppingCart(shoppingCartId, cartItemId, cancellation);

            // Assert
            Assert.DoesNotContain(cart.Items, item => item.CartItemId == cartItemId);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveCartItemFromShoppingCart_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var shoppingCartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.RemoveCartItemFromShoppingCart(shoppingCartId, cartItemId, cancellation));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RemoveCartItemFromShoppingCartUseCase(null));
        }
    }
}
