using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.CreateAnonymousCart;
using Moq;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class CreateAnonymousShoppingCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly CreateAnonymousShoppingCartUseCase _useCase;

        public CreateAnonymousShoppingCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new CreateAnonymousShoppingCartUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateAnonymousShoppingCart_ShouldCreateAndSaveCart()
        {
            // Arrange
            var cancellation = CancellationToken.None;

            // Act
            var response = await _useCase.CreateAnonymousShoppingCart(cancellation);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.ShoppingCartId);
            Assert.NotEqual(Guid.Empty, response.ShoppingCartId);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CreateAnonymousShoppingCartUseCase(null));
        }
    }
}
