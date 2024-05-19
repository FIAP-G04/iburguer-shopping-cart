using iBurguer.ShoppingCart.Core;

namespace iBurguer.ShoppingCart.UnitTests.Core
{
    public class ExceptionTest
    {
        [Fact]
        public void InvalidPriceException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.InvalidPriceException>(() => Exceptions.InvalidPriceException.ThrowIf(true));
            Assert.Equal("The price cannot have a value equal to zero or negative", exception.Message);
        }

        [Fact]
        public void InvalidProductTypeException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.InvalidProductTypeException>(() => Exceptions.InvalidProductTypeException.ThrowIf(true));
            Assert.Equal("The product type provided is not valid", exception.Message);
        }

        [Fact]
        public void InvalidQuantityException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.InvalidQuantityException>(() => Exceptions.InvalidQuantityException.ThrowIf(true));
            Assert.Equal("A value greater than zero must be provided for the quantity field.", exception.Message);
        }

        [Fact]
        public void ItemNotPresentInCartException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.ItemNotPresentInCartException>(() => Exceptions.ItemNotPresentInCartException.ThrowIf(true));
            Assert.Equal("The item with Id provided does not exist in the cart.", exception.Message);
        }

        [Fact]
        public void CannotUpdateClosedCartException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.CannotUpdateClosedCartException>(() => Exceptions.CannotUpdateClosedCartException.ThrowIf(true));
            Assert.Equal("The shopping cart with Id provided is already closed and cannot be modified.", exception.Message);
        }

        [Fact]
        public void UnableToCloseWithoutAnyCartItemsException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.UnableToCloseWithoutAnyCartItemsException>(() => Exceptions.UnableToCloseWithoutAnyCartItemsException.ThrowIf(true));
            Assert.Equal("It is not possible to close a shopping cart with Id provided without any items", exception.Message);
        }

        [Fact]
        public void InvalidCustomerIdException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.InvalidCustomerIdException>(() => Exceptions.InvalidCustomerIdException.ThrowIf(true));
            Assert.Equal("Invalid customer Id.", exception.Message);
        }

        [Fact]
        public void ShoppingCartNotFoundException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.ShoppingCartNotFoundException>(() => Exceptions.ShoppingCartNotFoundException.ThrowIf(true));
            Assert.Equal("No shopping cart was found with the provided Id.", exception.Message);
        }

        [Fact]
        public void ProductNotFoundException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.ProductNotFoundException>(() => Exceptions.ProductNotFoundException.ThrowIf(true));
            Assert.Equal("No product was found with the provided Id.", exception.Message);
        }

        [Fact]
        public void ProductNotPresentInCartException_ShouldThrowWithCorrectMessage()
        {
            // Act & Assert
            var exception = Assert.Throws<Exceptions.ProductNotPresentInCartException>(() => Exceptions.ProductNotPresentInCartException.ThrowIf(true));
            Assert.Equal("The product with Id provided does not exist in the cart.", exception.Message);
        }
    }
}
