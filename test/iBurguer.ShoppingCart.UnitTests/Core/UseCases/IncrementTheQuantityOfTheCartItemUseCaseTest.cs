using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.IncrementCartItem;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class IncrementTheQuantityOfTheCartItemUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly IncrementTheQuantityOfTheCartItemUseCase _useCase;

        public IncrementTheQuantityOfTheCartItemUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new IncrementTheQuantityOfTheCartItemUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task IncrementTheQuantityOfTheCartItem_ShouldIncrementQuantityAndSaveCart()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var cart = Cart.CreateAnonymousShoppingCart();
            cart.AddCartItem(new Product(Guid.NewGuid(), "Test Product", ProductType.MainDish, new Price(10)), new Quantity(5));
            var shoppingCartId = cart.Id;
            var cartItemId = cart.Items.First().CartItemId;
            var request = new IncrementTheQuantityOfTheCartItemRequest(shoppingCartId, cartItemId, 1);

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _repositoryMock.Setup(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.IncrementTheQuantityOfTheCartItem(request, cancellation);

            // Assert
            var updatedCartItem = cart.GetCartItemById(cartItemId);
            Assert.NotNull(updatedCartItem);
            Assert.Equal(6, updatedCartItem.Quantity);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IncrementTheQuantityOfTheCartItem_ShouldThrowShoppingCartNotFoundException_WhenShoppingCartDoesNotExist()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var shoppingCartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();
            var request = new IncrementTheQuantityOfTheCartItemRequest(shoppingCartId, cartItemId, 1);

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.IncrementTheQuantityOfTheCartItem(request, cancellation));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new IncrementTheQuantityOfTheCartItemUseCase(null));
        }
    }
}
