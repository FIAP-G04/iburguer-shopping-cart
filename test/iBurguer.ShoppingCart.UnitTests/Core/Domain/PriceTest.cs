using iBurguer.ShoppingCart.Core.Domain;
using System.Globalization;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.Domain
{
    public class PriceTest
    {
        [Fact]
        public void Price_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            decimal amount = 10.5m;

            // Act
            var price = new Price(amount);

            // Assert
            Assert.Equal(amount, price.Amount);
        }

        [Fact]
        public void Price_Constructor_ShouldThrowInvalidPriceException_ForNegativeAmount()
        {
            // Arrange
            decimal negativeAmount = -1m;

            // Act & Assert
            Assert.Throws<InvalidPriceException>(() => new Price(negativeAmount));
        }

        [Fact]
        public void Price_ToString_ShouldReturnAmountAsString()
        {
            // Arrange
            decimal amount = 10.5m;
            var price = new Price(amount);
            var expectedString = amount.ToString(CultureInfo.InvariantCulture);

            // Act
            var result = price.ToString();

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ImplicitConversion_ToDecimal_ShouldReturnAmount()
        {
            // Arrange
            decimal amount = 10.5m;
            var price = new Price(amount);

            // Act
            decimal result = price;

            // Assert
            Assert.Equal(amount, result);
        }

        [Fact]
        public void ImplicitConversion_FromDecimal_ShouldReturnPrice()
        {
            // Arrange
            decimal amount = 10.5m;

            // Act
            Price price = amount;

            // Assert
            Assert.Equal(amount, price.Amount);
        }
    }
}
