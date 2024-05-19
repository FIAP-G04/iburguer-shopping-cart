using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.Abstractions
{
    public class DomainExceptionTest
    {
        [Fact]
        public void New_ShouldReturnNewInstanceOfException()
        {
            // Act
            var exception = InvalidPriceException.New;

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidPriceException>(exception);
        }

        [Fact]
        public void ThrowIf_ShouldThrowExceptionWhenConditionIsTrue()
        {
            // Arrange
            bool condition = true;

            // Act & Assert
            var exception = Assert.Throws<InvalidPriceException>(() => InvalidPriceException.ThrowIf(condition));
            Assert.Equal("The price cannot have a value equal to zero or negative", exception.Message);
        }

        [Fact]
        public void ThrowIf_ShouldNotThrowExceptionWhenConditionIsFalse()
        {
            // Arrange
            bool condition = false;

            // Act & Assert
            InvalidPriceException.ThrowIf(condition);
        }

        [Fact]
        public void ThrowIfNull_ShouldThrowExceptionWhenObjectIsNull()
        {
            // Arrange
            object obj = null;

            // Act & Assert
            var exception = Assert.Throws<InvalidPriceException>(() => InvalidPriceException.ThrowIfNull(obj));
            Assert.Equal("The price cannot have a value equal to zero or negative", exception.Message);
        }

        [Fact]
        public void ThrowIfNull_ShouldNotThrowExceptionWhenObjectIsNotNull()
        {
            // Arrange
            object obj = new object();

            // Act & Assert
            InvalidPriceException.ThrowIfNull(obj);
        }

        [Fact]
        public void Throw_ShouldThrowException()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidPriceException>(() => InvalidPriceException.Throw());
            Assert.Equal("The price cannot have a value equal to zero or negative", exception.Message);
        }
    }
}
