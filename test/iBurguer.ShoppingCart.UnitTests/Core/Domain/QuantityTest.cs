using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.Domain
{
    public class QuantityTest
    {
        [Fact]
        public void Quantity_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            ushort value = 5;

            // Act
            var quantity = new Quantity(value);

            // Assert
            Assert.Equal(value, quantity.Value);
        }

        [Fact]
        public void Quantity_Constructor_ShouldThrowInvalidQuantityException_ForValueLessThanOne()
        {
            // Arrange
            ushort invalidValue = 0;

            // Act & Assert
            Assert.Throws<InvalidQuantityException>(() => new Quantity(invalidValue));
        }

        [Fact]
        public void Quantity_ImplicitConversion_ToUShort_ShouldReturnValue()
        {
            // Arrange
            ushort value = 10;
            var quantity = new Quantity(value);

            // Act
            ushort result = quantity;

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Quantity_ImplicitConversion_FromUShort_ShouldReturnQuantity()
        {
            // Arrange
            ushort value = 10;

            // Act
            Quantity quantity = value;

            // Assert
            Assert.Equal(value, quantity.Value);
        }

        [Fact]
        public void Quantity_ToString_ShouldReturnValueAsString()
        {
            // Arrange
            ushort value = 10;
            var quantity = new Quantity(value);

            // Act
            var result = quantity.ToString();

            // Assert
            Assert.Equal("10", result);
        }

        [Fact]
        public void Quantity_IsMinimum_ShouldReturnTrue_WhenValueIsOne()
        {
            // Arrange
            var quantity = new Quantity(1);

            // Act
            var result = quantity.IsMinimum();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Quantity_IsMinimum_ShouldReturnFalse_WhenValueIsGreaterThanOne()
        {
            // Arrange
            var quantity = new Quantity(2);

            // Act
            var result = quantity.IsMinimum();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Quantity_Increment_ShouldIncreaseValueByOne()
        {
            // Arrange
            var quantity = new Quantity(5);

            // Act
            quantity.Increment();

            // Assert
            Assert.Equal(6, quantity.Value);
        }

        [Fact]
        public void Quantity_Increment_ByAnotherQuantity_ShouldIncreaseValueCorrectly()
        {
            // Arrange
            var quantity1 = new Quantity(5);
            var quantity2 = new Quantity(3);

            // Act
            quantity1.Increment(quantity2);

            // Assert
            Assert.Equal(8, quantity1.Value);
        }

        [Fact]
        public void Quantity_Decrement_ShouldDecreaseValueByOne()
        {
            // Arrange
            var quantity = new Quantity(5);

            // Act
            quantity.Decrement();

            // Assert
            Assert.Equal(4, quantity.Value);
        }

        [Fact]
        public void Quantity_Decrement_ByAnotherQuantity_ShouldDecreaseValueCorrectly()
        {
            // Arrange
            var quantity1 = new Quantity(5);
            var quantity2 = new Quantity(3);

            // Act
            quantity1.Decrement(quantity2);

            // Assert
            Assert.Equal(2, quantity1.Value);
        }

        [Fact]
        public void Quantity_Decrement_ShouldThrowInvalidQuantityException_WhenValueBecomesLessThanOne()
        {
            // Arrange
            var quantity = new Quantity(1);

            // Act & Assert
            Assert.Throws<InvalidQuantityException>(() => quantity.Decrement());
        }
    }
}
