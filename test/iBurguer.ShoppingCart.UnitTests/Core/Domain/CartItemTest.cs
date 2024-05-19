using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.UnitTests.Core.Domain
{
    public class CartItemTest
    {
        [Fact]
        public void CartItem_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(2);

            // Act
            var cartItem = new CartItem(product, quantity);

            // Assert
            Assert.NotNull(cartItem);
            Assert.Equal(product, cartItem.Product);
            Assert.Equal(quantity, cartItem.Quantity);
            Assert.Equal(10, cartItem.UnitPrice.Amount);
            Assert.Equal(20, cartItem.Subtotal.Amount);
        }

        [Fact]
        public void CartItem_UnitPrice_ShouldReturnProductPrice()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), "tst", ProductType.Dessert, new Price(15));
            var quantity = new Quantity(1);
            var cartItem = new CartItem(product, quantity);

            // Act
            var unitPrice = cartItem.UnitPrice;

            // Assert
            Assert.Equal(15, unitPrice.Amount);
        }

        [Fact]
        public void CartItem_Subtotal_ShouldReturnCorrectSubtotal()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(3);
            var cartItem = new CartItem(product, quantity);

            // Act
            var subtotal = cartItem.Subtotal;

            // Assert
            Assert.Equal(30, subtotal.Amount);
        }

        [Fact]
        public void CartItem_UpdatePrice_ShouldUpdateProductPrice()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);
            var cartItem = new CartItem(product, quantity);
            var newPrice = new Price(20);

            // Act
            cartItem.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(20, cartItem.UnitPrice.Amount);
            Assert.Equal(20, cartItem.Subtotal.Amount);
        }
    }
}
