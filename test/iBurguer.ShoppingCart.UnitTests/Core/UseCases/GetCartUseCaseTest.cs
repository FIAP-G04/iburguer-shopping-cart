using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.GetCart;
using Moq;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class GetCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly GetCartUseCase _useCase;

        public GetCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new GetCartUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetCartById_ShouldReturnShoppingCartResponse_WhenCartExists()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var cart = Cart.CreateAnonymousShoppingCart();
            var expectedResponse = ShoppingCartResponse.Convert(cart);
            var shoppingCartId = cart.Id;

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            // Act
            var response = await _useCase.GetCartById(shoppingCartId, cancellation);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedResponse.ShoppingCartId, response.ShoppingCartId);
        }

        [Fact]
        public async Task GetCartById_ShouldThrowShoppingCartNotFoundException_WhenCartDoesNotExist()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var shoppingCartId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<ShoppingCartNotFoundException>(() =>
                _useCase.GetCartById(shoppingCartId, cancellation));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GetCartUseCase(null));
        }
    }
}
