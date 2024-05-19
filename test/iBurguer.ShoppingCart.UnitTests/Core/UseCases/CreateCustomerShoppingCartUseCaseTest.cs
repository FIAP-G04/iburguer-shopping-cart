using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Core.UseCases.CreateCustomerCart;
using Moq;

namespace iBurguer.ShoppingCart.UnitTests.Core.UseCases
{
    public class CreateCustomerShoppingCartUseCaseTest
    {
        private readonly Mock<IShoppingCartRepository> _repositoryMock;
        private readonly CreateCustomerShoppingCartUseCase _useCase;

        public CreateCustomerShoppingCartUseCaseTest()
        {
            _repositoryMock = new Mock<IShoppingCartRepository>();
            _useCase = new CreateCustomerShoppingCartUseCase(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateCustomerShoppingCart_ShouldCreateAndSaveCart()
        {
            // Arrange
            var cancellation = CancellationToken.None;
            var customerId = Guid.NewGuid();
            var request = new CreateShoppingCartRequest(customerId);

            // Act
            var response = await _useCase.CreateCustomerShoppingCart(request, cancellation);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.ShoppingCartId);
            Assert.NotEqual(Guid.Empty, response.ShoppingCartId);
            Assert.Equal(customerId, response.CustomerId);
            _repositoryMock.Verify(repo => repo.Save(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CreateCustomerShoppingCartUseCase(null));
        }
    }
}
