using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Infrastructure.Redis;
using iBurguer.ShoppingCart.Infrastructure.Repositories;
using Moq;

namespace iBurguer.ShoppingCart.UnitTests.Infrastructure.Repositories
{
    public class ShoppingCartRepositoryTest
    {
        private readonly Mock<ICacheContext> _cacheContextMock;
        private readonly ShoppingCartRepository _repository;

        public ShoppingCartRepositoryTest()
        {
            _cacheContextMock = new Mock<ICacheContext>();
            _repository = new ShoppingCartRepository(_cacheContextMock.Object);
        }

        [Fact]
        public async Task Save_ShouldCallSetWithCorrectParameters()
        {
            // Arrange
            var shoppingCart = new Cart
            {
                Id = Guid.NewGuid()
                // Populate other required properties
            };
            var cancellationToken = new CancellationToken();

            // Act
            await _repository.Save(shoppingCart, cancellationToken);

            // Assert
            _cacheContextMock.Verify(
                x => x.Set("cart", shoppingCart.Id.ToString(), shoppingCart, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var expectedCart = new Cart
            {
                Id = shoppingCartId
                // Populate other required properties
            };
            var cancellationToken = new CancellationToken();

            _cacheContextMock.Setup(x => x.Get<Cart>("cart", shoppingCartId.ToString(), cancellationToken))
                             .ReturnsAsync(expectedCart);

            // Act
            var result = await _repository.GetById(shoppingCartId, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCart, result);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenCartDoesNotExist()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _cacheContextMock.Setup(x => x.Get<Cart>("cart", shoppingCartId.ToString(), cancellationToken))
                             .ReturnsAsync((Cart?)null);

            // Act
            var result = await _repository.GetById(shoppingCartId, cancellationToken);

            // Assert
            Assert.Null(result);
        }
    }
}
